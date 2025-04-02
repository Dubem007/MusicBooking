using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Domain.Entites
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public string EventType { get; set; }
        public string VenueName { get; set; }
        public string VenueAddress { get; set; }
        public decimal Budget { get; set; }
        public string Status { get; set; } // Draft, Published, Canceled, Completed
        public Guid OrganizerId { get; set; }
        public AppUser Organizer { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Artist> InterestedArtists { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
