using MusicBooking.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Application.Interface
{
    public interface IEventService
    {
        Task<EventResponseDto> CreateEvent(CreateEventDto createEventDto);
        Task<EventResponseDto> DeleteEvent(Guid id);
        Task<EventDto> GetEvent(Guid eventId);
        Task<List<EventDto>> GetEvents();
        Task<EventResponseListDto> GetOrganizerEvents();
        Task<List<EventDto>> SearchEvents(string query, DateTime? startDate, DateTime? endDate, string eventType);
        Task<EventResponseDto> UpdateEvent(Guid id, CreateEventDto updateEventDto);
        Task<EventResponseDto> UpdateEventStatus(Guid id, string status);
    }
}
