using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Domain.Entites
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid ArtistId { get; set; }
        public Artist Artist { get; set; }
        public Guid ReviewerId { get; set; }
        public AppUser Reviewer { get; set; }
        public int Rating { get; set; } // 1-5
        public string Comment { get; set; }
        public Guid? BookingId { get; set; }
        public Booking Booking { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
