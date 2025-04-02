using MusicBooking.Domain.Entites;
using MusicBooking.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Infrastructure.Interface
{
    public interface IArtistRepository: IRepository<Artist>
    {
        Task<IEnumerable<Artist>> SearchArtistsByGenreAsync(string genre);
        Task<IEnumerable<Artist>> GetAvailableArtistsForDateRangeAsync(DateTimeOffset start, DateTimeOffset end);
        Task<Artist> GetArtistWithDetailsAsync(Guid id);
    }
}
