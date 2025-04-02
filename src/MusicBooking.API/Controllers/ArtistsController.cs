using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicBooking.Application.Interface;
using MusicBooking.Domain.Dtos;
using MusicBooking.Domain.Entites;
using MusicBooking.Infrastructure.Interface;

namespace MusicBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistsController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistDto>>> GetArtists()
        {
            var artists = await _artistService.GetArtists();
            if (artists == null)
            {
                return BadRequest(artists);
            }
            return Ok(artists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistDto>> GetArtist(Guid id)
        {
            var artist = await _artistService.GetArtist(id);
            if (artist == null)
            {
                return BadRequest(artist);
            }
            return Ok(artist);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ArtistDto>>> SearchArtists([FromQuery] string genre)
        {
            var artists = await _artistService.SearchArtists(genre);
            if (artists == null)
            {
                return BadRequest(artists);
            }
            return Ok(artists);
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<ArtistDto>>> GetAvailableArtists(
            [FromQuery] DateTimeOffset startTime,
            [FromQuery] DateTimeOffset endTime)
        {
            var artists = await _artistService.GetAvailableArtists(startTime, endTime);
            if (artists == null)
            {
                return BadRequest(artists);
            }
            return Ok(artists);
        }

        [HttpPost]
        public async Task<ActionResult<ArtistDto>> CreateArtist(CreateArtistDto createArtistDto)
        {
            var createdArtist = await _artistService.CreateArtist(createArtistDto);
            if (createdArtist == null)
            {
                return BadRequest(createdArtist);
            }
            return Ok(createdArtist);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArtist(Guid id, UpdateArtistDto updateArtistDto)
        {
           var resp =  await _artistService.UpdateArtist(id, updateArtistDto);
            if (resp == null)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist(Guid id)
        {
            var artist = await _artistService.DeleteArtist(id);
            if (artist == false)
            {
                return BadRequest(artist);
            }
            return Ok(artist);
        }
    }
}
