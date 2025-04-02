using Microsoft.EntityFrameworkCore;
using MusicBooking.Domain.Entites;
using MusicBooking.Infrastructure.Data;
using MusicBooking.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Infrastructure.Repository
{
    public class BookingRepository:Repository<Booking>, IBookingRepositoryy
    {
        public BookingRepository(MusicBookingDbContext context) : base(context) { }

        public async Task<List<Booking>> GetBookingsByArtistAsync(Guid artistId)
        {
            var resp = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Organizer)
                .Where(b => b.ArtistId == artistId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
            return resp;
        }

        public async Task<List<Booking>> GetBookingsByOrganizerAsync(Guid organizerId)
        {
            return await _context.Bookings
                .Include(b => b.Artist)
                .Include(b => b.Event)
                .Where(b => b.OrganizerId == organizerId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetBookingsByEventAsync(Guid eventId)
        {
            return await _context.Bookings
                .Include(b => b.Artist)
                .Include(b => b.Organizer)
                .Where(b => b.EventId == eventId)
                .ToListAsync();
        }

        public async Task<Booking> GetBookingWithDetailsAsync(Guid bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Artist)
                .Include(b => b.Event)
                .Include(b => b.Organizer)
                .Include(b => b.Payment)
                .Include(b => b.Contract)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<bool> HasArtistBookingConflictAsync(Guid artistId, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            return await _context.Bookings
                .Include(b => b.Event)
                .AnyAsync(b =>
                    b.ArtistId == artistId &&
                    b.Status != "Rejected" &&
                    b.Status != "Canceled" &&
                    ((b.Event.StartTime <= endTime && b.Event.EndTime >= startTime)));
        }
    }
}
