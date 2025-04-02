using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Domain.Entites
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid ArtistId { get; set; }
        public Artist Artist { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public Guid OrganizerId { get; set; }
        public AppUser Organizer { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } // Pending, Accepted, Rejected, Canceled, Completed
        public string Message { get; set; }
        public Payment Payment { get; set; }
        public Guid? ContractId { get; set; }
        public Contract Contract { get; set; }
        public DateTimeOffset BookingTime { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
