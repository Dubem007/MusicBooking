using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicBooking.Application.Interface;
using MusicBooking.Application.Services;
using MusicBooking.Domain.Dtos;
using MusicBooking.Domain.Entites;
using MusicBooking.Infrastructure.Interface;
using System.Security.Claims;

namespace MusicBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetBooking(Guid id)
        {
            var eventEntity = await _bookingService.GetBooking(id);

            if (eventEntity == null)
                return NotFound();

            return Ok(eventEntity);
        }

        [HttpGet("allbooking")]
        public async Task<ActionResult<List<BookingDto>>> GetBookings()
        {
            var events = await _bookingService.GetBookings();
            return Ok(events);
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> CreateEvent(CreateBookingDto createEventDto)
        {
            var createdEvent = await _bookingService.CreateBooking(createEventDto);
            if (createdEvent == null)
            {
                return BadRequest(createdEvent);
            }
            return Ok(createdEvent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, string status)
        {
            var updatedBooking = await _bookingService.UpdateBookingStatus(id, status);
            if (updatedBooking == null)
            {
                return BadRequest(updatedBooking);
            }
            return Ok(updatedBooking);
        }

    }
}
