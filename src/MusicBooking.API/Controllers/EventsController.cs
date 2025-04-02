using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicBooking.Application.Interface;
using MusicBooking.Domain.Dtos;
using MusicBooking.Domain.Entites;
using System.Security.Claims;

namespace MusicBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
        {
            var events = await _eventService.GetEvents();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent(Guid id)
        {
            var eventEntity = await _eventService.GetEvent(id);

            if (eventEntity == null)
                return NotFound();

            return Ok(eventEntity);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EventDto>>> SearchEvents(
            [FromQuery] string query,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string eventType)
        {
            var events = await _eventService.SearchEvents(query, startDate, endDate, eventType);
            return Ok(events);
        }

        [HttpGet("organizer")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetOrganizerEvents()
        {
            var events = await _eventService.GetOrganizerEvents();
            return Ok(events);
        }

        [HttpPost]
        public async Task<ActionResult<EventDto>> CreateEvent(CreateEventDto createEventDto)
        {
            var createdEvent = await _eventService.CreateEvent(createEventDto);
            if (createdEvent == null)
            {
                return BadRequest(createdEvent);
            }
            return Ok(createdEvent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, CreateEventDto updateEventDto)
        {
            var createdEvent = await _eventService.UpdateEvent(id, updateEventDto);
            if (createdEvent == null)
            {
                return BadRequest(createdEvent);
            }
            return Ok(createdEvent);
            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateEventStatus(Guid id, [FromBody] string status)
        {
            var createdEvent = await _eventService.UpdateEventStatus(id, status);
            if (createdEvent == null)
            {
                return BadRequest(createdEvent);
            }
            return Ok(createdEvent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var deletedEvent = await _eventService.DeleteEvent(id);
            if (deletedEvent == null)
            {
                return BadRequest(deletedEvent);
            }
            return Ok(deletedEvent);
        }
    }
}
