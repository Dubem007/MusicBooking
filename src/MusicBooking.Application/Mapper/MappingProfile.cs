using MusicBooking.Domain.Dtos;
using AutoMapper;
using MusicBooking.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MusicBooking.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<AppUser, UserDto>();
            CreateMap<RegisterUserDto, AppUser>();

            // Artist mappings
            CreateMap<Artist, ArtistDto>()
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
                    src.Reviews.Any() ? src.Reviews.Average(r => r.Rating) : 0));
            CreateMap<CreateArtistDto, Artist>();
            CreateMap<UpdateArtistDto, Artist>();

            // Event mappings
            CreateMap<Event, EventDto>()
                .ForMember(dest => dest.Organizer, opt => opt.MapFrom(src => src.Organizer));
            CreateMap<CreateEventDto, Event>();

            // Booking mappings
            CreateMap<Booking, BookingDto>();
            CreateMap<CreateBookingDto, Booking>();

            // Payment mappings
            CreateMap<Payment, PaymentDto>();
            CreateMap<CreatePaymentDto, Payment>();

            // Review mappings
            CreateMap<Review, ReviewDto>();
            CreateMap<CreateReviewDto, Review>();

            // Other mappings
            CreateMap<SocialMediaDetails, SocialMediaLinkDto>();
            CreateMap<PortfolioItem, PortfolioItemDto>();
        }
    }
}
