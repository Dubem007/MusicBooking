using Microsoft.AspNetCore.Mvc;
using MusicBooking.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Application.Interface
{
    public interface IArtistService
    {
        Task<ArtistDto> CreateArtist(CreateArtistDto createArtistDto);
        Task<bool> DeleteArtist(Guid id);
        Task<ArtistDto> GetArtist(Guid id);
        Task<List<ArtistDto>> GetArtists();
        Task<ActionResult<List<ArtistDto>>> GetAvailableArtists(DateTimeOffset startTime, DateTimeOffset endTime);
        Task<List<ArtistDto>> SearchArtists(string genre);
        Task<UpdateArtistResponseDto> UpdateArtist(Guid id, UpdateArtistDto updateArtistDto);
    }
}
