using MusicBooking.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Infrastructure.Interface
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<IEnumerable<Event>> GetEventsByOrganizerAsync(Guid organizerId);
        Task<Event> GetEventWithDetailsAsync(Guid eventId);
        Task<IEnumerable<Event>> GetUpcomingEventsAsync();
        Task<IEnumerable<Event>> SearchEventsAsync(string query, DateTime? startDate, DateTime? endDate, string eventType);
    }
}
