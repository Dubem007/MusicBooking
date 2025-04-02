using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicBooking.Application.Interface;
using MusicBooking.Domain.Dtos;
using MusicBooking.Domain.Entites;
using MusicBooking.Infrastructure.Interface;
using MusicBooking.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Application.Services
{
    public class BookingService: IBookingService
    {
        private readonly IBookingRepositoryy _bookingRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IAppSessionContextRepository _appSessionContextRepository;
        private readonly IMapper _mapper;

        public BookingService(
            IBookingRepositoryy bookingRepository,
            IEventRepository eventRepository,
            IArtistRepository artistRepository,
            IMapper mapper,
            IAppSessionContextRepository appSessionContextRepository)
        {
            _bookingRepository = bookingRepository;
            _eventRepository = eventRepository;
            _artistRepository = artistRepository;
            _mapper = mapper;
            _appSessionContextRepository = appSessionContextRepository;
        }

        public async Task<List<BookingDto>> GetBookings()
        {
            try
            {
                var userIdClaim = await _appSessionContextRepository.GetUserDetails();
                if (userIdClaim == null)
                    return new List<BookingDto>();

                var artistIdClaim = userIdClaim.IsOrganizer != true ? userIdClaim.UserId : Guid.Empty;
                bool isArtist = artistIdClaim != Guid.Empty ? true : false;

                var isOrganizerClaim = userIdClaim.IsOrganizer == true ? userIdClaim.UserId : Guid.Empty;
                bool isOrganizer = isOrganizerClaim != Guid.Empty ? true : false;

                var bookings = new List<Booking>();

                if (isArtist)
                {
                    bookings = await _bookingRepository.GetBookingsByArtistAsync(artistIdClaim);
                }
                else if (isOrganizer)
                {
                    bookings = await _bookingRepository.GetBookingsByOrganizerAsync(isOrganizerClaim);
                }

                var resp = _mapper.Map<List<BookingDto>>(bookings);

                return resp;
            }
            catch (Exception ex)
            {

              return new List<BookingDto>();
            }
            
        }

        public async Task<BookingDto> GetBooking(Guid id)
        {
            try
            {
                var userIdClaim = await _appSessionContextRepository.GetUserDetails();
                if (userIdClaim == null)
                    return new BookingDto();

                var booking = await _bookingRepository.GetBookingWithDetailsAsync(id);

                if (booking == null)
                    return new BookingDto();

                var artistIdClaim = userIdClaim.IsOrganizer != true ? userIdClaim.UserId : Guid.Empty;
                bool isArtist = artistIdClaim != Guid.Empty ? true : false;

                if (booking.OrganizerId != artistIdClaim && (!isArtist || booking.ArtistId != artistIdClaim))
                    return new BookingDto();

                var resp = _mapper.Map<BookingDto>(booking);

                return resp;
            }
            catch (Exception ex)
            {
                return new BookingDto();
            }
           
        }

        public async Task<BookingResponseDto> CreateBooking(CreateBookingDto createBookingDto)
        {
            try
            {
                var userIdClaim = await _appSessionContextRepository.GetUserDetails();
                if (userIdClaim == null)
                    return new BookingResponseDto() { Status = false, Message = "Kindly login to proceed to create booking", Data = new BookingDto() };

                var isOrganizerClaim = userIdClaim.IsOrganizer == true ? userIdClaim.UserId : Guid.Empty;
                bool isOrganizer = isOrganizerClaim != Guid.Empty ? true : false;
                if (isOrganizer == false)
                    return new BookingResponseDto() { Status = false, Message = "User is not an organizer and not permitted to create bookings", Data = new BookingDto() };

                var eventEntity = await _eventRepository.GetByIdAsync(createBookingDto.EventId);
                if (eventEntity == null)
                    return new BookingResponseDto() { Status = false, Message = "No event record found..", Data = new BookingDto() };

                if (eventEntity.OrganizerId != isOrganizerClaim)
                    return new BookingResponseDto() { Status = false, Message = "You can only create bookings for your own events", Data = new BookingDto() };

                var artist = await _artistRepository.GetByIdAsync(createBookingDto.ArtistId);
                if (artist == null)
                    return new BookingResponseDto() { Status = false, Message = "Artist doesn't exist", Data = new BookingDto() };

                // Check for artist booking conflicts
                if (await _bookingRepository.HasArtistBookingConflictAsync(createBookingDto.ArtistId, eventEntity.StartTime, eventEntity.EndTime))
                    return new BookingResponseDto() { Status = false, Message = "Artist has a scheduling conflict for this time period", Data = new BookingDto() };

                var booking = _mapper.Map<Booking>(createBookingDto);
                booking.OrganizerId = userIdClaim.UserId;
                booking.Status = "Pending";
                booking.BookingTime = DateTimeOffset.UtcNow;
                booking.CreatedAt = DateTimeOffset.UtcNow;
                booking.UpdatedAt = DateTimeOffset.UtcNow;

                var createdBooking = await _bookingRepository.AddAsync(booking);
                var resp = _mapper.Map<BookingDto>(createdBooking);
                return new BookingResponseDto() { Status = true, Message = $"Successfully created booking for artist {createBookingDto.ArtistId} ", Data = resp };
            }
            catch (Exception ex)
            {

                return new BookingResponseDto() { Status = true, Message = $"Failed to create booking for artist {createBookingDto.ArtistId} ", Data = new BookingDto() } ;
            }
          
        }

        public async Task<BookingResponseDto> UpdateBookingStatus(Guid id, string status)
        {
            try
            {
                var userIdClaim = await _appSessionContextRepository.GetUserDetails();
                if (userIdClaim == null)
                    return new BookingResponseDto() { Status = false, Message = "Kindly login to proceed to create booking", Data = new BookingDto() };


                var booking = await _bookingRepository.GetByIdAsync(id);

                if (booking == null)
                    return new BookingResponseDto() { Status = false, Message = "No record found for the booking", Data = new BookingDto() };

                var isArtistClaim = userIdClaim.IsOrganizer != true ? userIdClaim.UserId : Guid.Empty;
                bool isArtist = isArtistClaim != Guid.Empty ? true : false;

                // Validate status
                if (status != "Accepted" && status != "Rejected" && status != "Canceled" && status != "Completed")
                    return new BookingResponseDto() { Status = false, Message = "Invalid Status Value", Data = new BookingDto() };


                if ((status == "Accepted" || status == "Rejected") && (!isArtist || booking.ArtistId != userIdClaim.UserId))
                    return new BookingResponseDto() { Status = false, Message = "Only the booked artist can accept or reject a booking", Data = new BookingDto() };

                // Only organizer can cancel or complete a booking
                if ((status == "Canceled" || status == "Completed") && booking.OrganizerId != userIdClaim.UserId)
                    return new BookingResponseDto() { Status = false, Message = "Only the organizer can cancel or complete a booking", Data = new BookingDto() };

                booking.Status = status;
                booking.UpdatedAt = DateTimeOffset.UtcNow;

                await _bookingRepository.UpdateAsync(booking);
                return new BookingResponseDto() { Status = true, Message = "Bookings updated successfully", Data = new BookingDto() };

            }
            catch (Exception ex)
            {
                return new BookingResponseDto() { Status = true, Message = $"Failed to update bookings due to error: {ex.Message}", Data = new BookingDto() };
            }
          
        }
    }
}
