using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicBooking.Application.Interface;
using MusicBooking.Domain.Dtos;
using MusicBooking.Domain.Entites;
using MusicBooking.Infrastructure.Interface;
using MusicBooking.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Application.Services
{
    public class ArtistService: IArtistService
    {
        private readonly IArtistRepository _artistRepo;
        private readonly IMapper _mapper;
        public ArtistService(IArtistRepository artistRepo, IMapper mapper)
        {
            _artistRepo = artistRepo;
            _mapper = mapper;
        }

       
        public async Task<List<ArtistDto>> GetArtists()
        {
            try
            {
                var artists = await _artistRepo.GetAllAsync();
                var resp = _mapper.Map<List<ArtistDto>>(artists);

                return resp;
            }
            catch (Exception ex)
            {
                return new List<ArtistDto>();
            }
            
        }

        public async Task<ArtistDto> GetArtist(Guid id)
        {
            try
            {
                var artist = await _artistRepo.GetArtistWithDetailsAsync(id);

                if (artist == null)
                    return new ArtistDto();

                var resp = _mapper.Map<ArtistDto>(artist);

                return resp;
            }
            catch (Exception ex)
            {
                return new ArtistDto();
            }
            
        }

        public async Task<List<ArtistDto>> SearchArtists(string genre)
        {
            try
            {
                var artists = await _artistRepo.SearchArtistsByGenreAsync(genre);
                var resp = _mapper.Map<List<ArtistDto>>(artists);

                return resp;
            }
            catch (Exception ex)
            {
                return new List<ArtistDto> { new ArtistDto() };
            }
           
        }


        public async Task<ActionResult<List<ArtistDto>>> GetAvailableArtists(DateTimeOffset startTime,DateTimeOffset endTime)
        {
            try
            {
                var artists = await _artistRepo.GetAvailableArtistsForDateRangeAsync(startTime, endTime);
                var resp = _mapper.Map<List<ArtistDto>>(artists);

                return resp;
            }
            catch (Exception ex)
            {
                return new List<ArtistDto>();
            }
        
        }

        public async Task<ArtistDto> CreateArtist(CreateArtistDto createArtistDto)
        {
            try
            {
                var artist = _mapper.Map<Artist>(createArtistDto);
                artist.CreatedAt = DateTimeOffset.UtcNow;
                artist.UpdatedAt = DateTimeOffset.UtcNow;

                var createdArtist = await _artistRepo.AddAsync(artist);

                var resp =  _mapper.Map<ArtistDto>(createdArtist);
                resp.Id = createdArtist.Id;

                return resp;
            }
            catch (Exception ex)
            {

                return new ArtistDto();
            }
           
        }

        public async Task<UpdateArtistResponseDto> UpdateArtist(Guid id, UpdateArtistDto updateArtistDto)
        {
           
            try
            {
                var artist = await _artistRepo.GetByIdAsync(id);

                if (artist == null)
                    return new UpdateArtistResponseDto();

                _mapper.Map(updateArtistDto, artist);
                artist.UpdatedAt = DateTimeOffset.UtcNow;

                await _artistRepo.UpdateAsync(artist);

                var resp = _mapper.Map<UpdateArtistResponseDto>(updateArtistDto);
                resp.Message = "Successfully updated artist profile";
                return resp;
            }
            catch (Exception ex)
            {

                return new UpdateArtistResponseDto();
            }

        }

        public async Task<bool> DeleteArtist(Guid id)
        {
            try
            {
                var artist = await _artistRepo.GetByIdAsync(id);

                if (artist == null)
                    return false;

                await _artistRepo.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }
    }
}
