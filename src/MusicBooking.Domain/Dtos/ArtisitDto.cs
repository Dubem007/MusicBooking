using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Domain.Dtos
{
    public class ArtistDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Genre { get; set; }
        public decimal HourlyRate { get; set; }
        public string ProfilePictureUrl { get; set; }
        public ICollection<SocialMediaLinkDto> SocialMediaLinks { get; set; }
        public ICollection<PortfolioItemDto> PortfolioItems { get; set; }
        public double AverageRating { get; set; }
    }

    public class CreateArtistDto
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Genre { get; set; }
        public decimal HourlyRate { get; set; }
        public string ProfilePictureUrl { get; set; }
    }

    public class UpdateArtistDto
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Genre { get; set; }
        public decimal HourlyRate { get; set; }
        public string ProfilePictureUrl { get; set; }
    }

    public class UpdateArtistResponseDto
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Genre { get; set; }
        public decimal HourlyRate { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Message { get; set; }
    }

    public class SocialMediaLinkDto
    {
        public int Id { get; set; }
        public string Platform { get; set; }
        public string Url { get; set; }
    }

    public class PortfolioItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MediaUrl { get; set; }
        public string MediaType { get; set; }
    }

    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public string EventType { get; set; }
        public string VenueName { get; set; }
        public string VenueAddress { get; set; }
        public decimal Budget { get; set; }
        public string Status { get; set; }
        public UserDto Organizer { get; set; }
    }

    public class CreateEventDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public string EventType { get; set; }
        public string VenueName { get; set; }
        public string VenueAddress { get; set; }
        public decimal Budget { get; set; }
    }

    public class BookingDto
    {
        public int Id { get; set; }
        public ArtistDto Artist { get; set; }
        public EventDto Event { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public PaymentDto Payment { get; set; }
        public DateTimeOffset BookingTime { get; set; }
    }

    public class BookingResponseDto
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public BookingDto Data { get; set; }
    }

    public class EventResponseDto
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public EventDto Data { get; set; }
    }

    public class EventResponseListDto
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<EventDto> Data { get; set; }
    }

    public class CreateBookingDto
    {
        public Guid ArtistId { get; set; }
        public Guid EventId { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }
    }

    public class PaymentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public DateTimeOffset PaymentDate { get; set; }
    }

    public class CreatePaymentDto
    {
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentToken { get; set; }
    }

    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsOrganizer { get; set; }
    }

    public class RegisterUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsOrganizer { get; set; }
    }

    public class LoginUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthResponseDto
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public UserDto User { get; set; }
    }

    public class ReviewDto
    {
        public int Id { get; set; }
        public UserDto Reviewer { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class CreateReviewDto
    {
        public int ArtistId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public int? BookingId { get; set; }
    }

    public class PasswordHashDetails
    {
        public string Salt { get; set; }
        public string HashedValue { get; set; }
    }

    public class AppSettings
    {
        public int? PasswordHashIteration { get; set; }
        public string? PepperKey { get; set; }
        public string TokenLifeSpan { get; set; }
        public string? JwtAudience { get; set; }
        public string? JwtIssuer { get; set; }
        public string? JwtSecret { get; set; }
        public int JwtTokenTimeout { get; set; }
    }

    public class LoginViewModel
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public DateTime JwtTokenExpiry { get; set; }
        public string Message { get; set; } = null!;
    }

}
