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
    public class ArtistRepository : Repository<Artist>, IArtistRepository
    {
        public ArtistRepository(MusicBookingDbContext context) : base(context) { }

        public async Task<IEnumerable<Artist>> SearchArtistsByGenreAsync(string genre)
        {
            return await _context.Artists
                .Where(a => a.Genre.Contains(genre))
                .ToListAsync();
        }

        public async Task<IEnumerable<Artist>> GetAvailableArtistsForDateRangeAsync(DateTimeOffset start, DateTimeOffset end)
        {
            var busyArtistIds = await _context.Bookings
                .Where(b =>
                    b.Status != "Rejected" &&
                    b.Status != "Canceled" &&
                    ((b.Event.StartTime <= end && b.Event.EndTime >= start)))
                .Select(b => b.ArtistId)
                .Distinct()
                .ToListAsync();

            return await _context.Artists
                .Where(a => !busyArtistIds.Contains(a.Id))
                .ToListAsync();
        }

        public async Task<Artist> GetArtistWithDetailsAsync(Guid id)
        {
            var artists =  await _context.Artists
                .Include(a => a.SocialMediaLinks)
                .Include(a => a.PortfolioItems)
                .Include(a => a.Reviews)
                .FirstOrDefaultAsync(a => a.Id == id);
            return artists;
        }
    }
}
