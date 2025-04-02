using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Domain.Entites
{
    public class Artist
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Genre { get; set; }
        public decimal HourlyRate { get; set; }
        public string ProfilePictureUrl { get; set; }
        public ICollection<SocialMediaDetails> SocialMediaLinks { get; set; }
        public ICollection<PortfolioItem> PortfolioItems { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
