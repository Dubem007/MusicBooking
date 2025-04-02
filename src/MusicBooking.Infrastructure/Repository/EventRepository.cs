using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicBooking.Domain.Dtos;
using MusicBooking.Domain.Entites;
using MusicBooking.Infrastructure.Data;
using MusicBooking.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Infrastructure.Repository
{
    public class EventRepository: Repository<Event>, IEventRepository
    {
        public EventRepository(MusicBookingDbContext context) : base(context) { }

        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
        {
            return await _context.Events
                .Where(e => e.StartTime > DateTimeOffset.UtcNow && e.Status == "Published")
                .OrderBy(e => e.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByOrganizerAsync(Guid organizerId)
        {
            return await _context.Events
                .Where(e => e.OrganizerId == organizerId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<Event> GetEventWithDetailsAsync(Guid eventId)
        {
            return await _context.Events
                .Include(e => e.Organizer)
                .Include(e => e.Bookings)
                    .ThenInclude(b => b.Artist)
                .Include(e => e.InterestedArtists)
                .FirstOrDefaultAsync(e => e.Id == eventId);
        }

        public async Task<IEnumerable<Event>> SearchEventsAsync(
            string query,
            DateTime? startDate,
            DateTime? endDate,
            string eventType)
        {
            var events = _context.Events.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                events = events.Where(e =>
                    e.Title.Contains(query) ||
                    e.Description.Contains(query) ||
                    e.VenueName.Contains(query) ||
                    e.VenueAddress.Contains(query));
            }

            if (startDate.HasValue)
            {
                var startDateOffset = new DateTimeOffset(startDate.Value);
                events = events.Where(e => e.StartTime >= startDateOffset);
            }

            if (endDate.HasValue)
            {
                var endDateOffset = new DateTimeOffset(endDate.Value);
                events = events.Where(e => e.StartTime <= endDateOffset);
            }

            if (!string.IsNullOrWhiteSpace(eventType))
            {
                events = events.Where(e => e.EventType == eventType);
            }

            // Only return published events
            events = events.Where(e => e.Status == "Published");

            return await events
                .OrderBy(e => e.StartTime)
                .ToListAsync();
        }
    }
}
