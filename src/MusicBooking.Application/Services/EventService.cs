using AutoMapper;
using MusicBooking.Application.Interface;
using MusicBooking.Domain.Dtos;
using MusicBooking.Domain.Entites;
using MusicBooking.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Application.Services
{
    public class EventService: IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAppSessionContextRepository _appSessionContextRepository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IMapper mapper, IAppSessionContextRepository appSessionContextRepository)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _appSessionContextRepository = appSessionContextRepository;
        }

        public async Task<List<EventDto>> GetEvents()
        {
            var events = await _eventRepository.GetUpcomingEventsAsync();
            var resp = _mapper.Map<List<EventDto>>(events);
            return resp;
        }

        public async Task<EventDto> GetEvent(Guid eventId)
        {
            var eventEntity = await _eventRepository.GetEventWithDetailsAsync(eventId);

            if (eventEntity == null)
                return new EventDto();

            var resp = _mapper.Map<EventDto>(eventEntity);

            return resp;
        }


        public async Task<List<EventDto>> SearchEvents(
            string query,
            DateTime? startDate,
            DateTime? endDate,
            string eventType)
        {
            var events = await _eventRepository.SearchEventsAsync(query, startDate, endDate, eventType);
            var resp =_mapper.Map<IEnumerable<EventDto>>(events);

            return resp.ToList();
        }

        public async Task<EventResponseListDto> GetOrganizerEvents()
        {
            var userIdClaim = await _appSessionContextRepository.GetUserDetails();
            if (userIdClaim == null)
                return new EventResponseListDto() { Status = false, Message = "Kindly login to proceed to create booking", Data = new List<EventDto>() };

            var events = await _eventRepository.GetEventsByOrganizerAsync(userIdClaim.UserId);
            var resp = _mapper.Map<List<EventDto>>(events);

            return new EventResponseListDto() { Status = false, Message = "Successfully retrieved organizers events", Data = resp };
        }

        public async Task<EventResponseDto> CreateEvent(CreateEventDto createEventDto)
        {
            try
            {
                var userIdClaim = await _appSessionContextRepository.GetUserDetails();
                if (userIdClaim == null)
                    return new EventResponseDto() { Status = false, Message = "Kindly login to proceed to create booking", Data = new EventDto() };

                var isOrganizerClaim = userIdClaim.IsOrganizer == true ? userIdClaim.UserId : Guid.Empty;
                bool isOrganizer = isOrganizerClaim != Guid.Empty ? true : false;
                if (isOrganizer == false)
                    return new EventResponseDto() { Status = false, Message = "User is not an organizer and not permitted to create events", Data = new EventDto() };

                var eventEntity = _mapper.Map<Event>(createEventDto);
                eventEntity.OrganizerId = userIdClaim.UserId;
                eventEntity.Status = "Draft";
                eventEntity.CreatedAt = DateTimeOffset.UtcNow;
                eventEntity.UpdatedAt = DateTimeOffset.UtcNow;

                var createdEvent = await _eventRepository.AddAsync(eventEntity);
                var resp = _mapper.Map<EventDto>(createdEvent);
                return new EventResponseDto() { Status = false, Message = "Successfully created event", Data = resp };
            }
            catch (Exception ex)
            {
                return new EventResponseDto() { Status = false, Message = $"Failed to create event due to error : {ex.Message}", Data = new EventDto() };
            }
          
        }

        public async Task<EventResponseDto> UpdateEvent(Guid id, CreateEventDto updateEventDto)
        {
            try
            {
                var userIdClaim = await _appSessionContextRepository.GetUserDetails();
                if (userIdClaim == null)
                    return new EventResponseDto() { Status = false, Message = "Kindly login to proceed to create booking", Data = new EventDto() };

                var eventEntity = await _eventRepository.GetByIdAsync(id);

                if (eventEntity == null)
                    return new EventResponseDto() { Status = false, Message = "No event record found", Data = new EventDto() };

                if (eventEntity.OrganizerId != userIdClaim.UserId)
                    return new EventResponseDto() { Status = false, Message = "You can only update your own events", Data = new EventDto() };

                _mapper.Map(updateEventDto, eventEntity);
                eventEntity.UpdatedAt = DateTimeOffset.UtcNow;

                await _eventRepository.UpdateAsync(eventEntity);
                return new EventResponseDto() { Status = true, Message = "Successfully updated event ", Data = new EventDto() };
            }
            catch (Exception ex)
            {
                return new EventResponseDto() { Status = false, Message = $"Failed to update event record due to error : {ex.Message}", Data = new EventDto() };
            }
        
        }

        public async Task<EventResponseDto> UpdateEventStatus(Guid id, string status)
        {
            try
            {
                var userIdClaim = await _appSessionContextRepository.GetUserDetails();
                if (userIdClaim == null)
                    return new EventResponseDto() { Status = false, Message = "Kindly login to proceed to create booking", Data = new EventDto() };

                var eventEntity = await _eventRepository.GetByIdAsync(id);

                if (eventEntity == null)
                    return new EventResponseDto() { Status = false, Message = $"No event record found for Id : {id}", Data = new EventDto() };

                if (eventEntity.OrganizerId != userIdClaim.UserId)
                    return new EventResponseDto() { Status = false, Message = "You can only update your own events", Data = new EventDto() };

                // Validate status
                if (status != "Draft" && status != "Published" && status != "Canceled" && status != "Completed")
                    return new EventResponseDto() { Status = false, Message = "Invalid status value", Data = new EventDto() };

                eventEntity.Status = status;
                eventEntity.UpdatedAt = DateTimeOffset.UtcNow;

                await _eventRepository.UpdateAsync(eventEntity);
                return new EventResponseDto() { Status = false, Message = "Successfully updated event status", Data = new EventDto() };
            }
            catch (Exception ex)
            {

                return new EventResponseDto() { Status = false, Message = "Failed to update event status", Data = new EventDto() }; 
            }
           
        }

        public async Task<EventResponseDto> DeleteEvent(Guid id)
        {
            try
            {
                var userIdClaim = await _appSessionContextRepository.GetUserDetails();
                if (userIdClaim == null)
                    return new EventResponseDto() { Status = false, Message = "Kindly login to proceed to create booking", Data = new EventDto() };


                var eventEntity = await _eventRepository.GetByIdAsync(id);

                if (eventEntity == null)
                    return new EventResponseDto() { Status = false, Message = $"No event record found for Id : {id}", Data = new EventDto() };

                if (eventEntity.OrganizerId != userIdClaim.UserId)
                    return new EventResponseDto() { Status = false, Message = "You can only delete your own events", Data = new EventDto() };

                await _eventRepository.DeleteAsync(id);
                return new EventResponseDto() { Status = false, Message = "Successfully deleted event", Data = new EventDto() };
            }
            catch (Exception ex)
            {
                return new EventResponseDto() { Status = false, Message = $"Failed to delete event due to error {ex.Message}", Data = new EventDto() }; ;
            }
           
        }
    }

}
