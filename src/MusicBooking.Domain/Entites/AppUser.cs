using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Domain.Entites
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public bool IsOrganizer { get; set; }
        public Guid? ArtistId { get; set; }
        public bool IsActive { get; set; }
        public Artist Artist { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Event> OrganizedEvents { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
