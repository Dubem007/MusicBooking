using MusicBooking.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Infrastructure.Interface
{
    public interface IBookingRepositoryy : IRepository<Booking>
    {
        Task<List<Booking>> GetBookingsByArtistAsync(Guid artistId);
        Task<List<Booking>> GetBookingsByEventAsync(Guid eventId);
        Task<List<Booking>> GetBookingsByOrganizerAsync(Guid organizerId);
        Task<Booking> GetBookingWithDetailsAsync(Guid bookingId);
        Task<bool> HasArtistBookingConflictAsync(Guid artistId, DateTimeOffset startTime, DateTimeOffset endTime);
    }
}
