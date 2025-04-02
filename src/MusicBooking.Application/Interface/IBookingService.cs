using MusicBooking.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Application.Interface
{
    public interface IBookingService
    {
        Task<BookingResponseDto> CreateBooking(CreateBookingDto createBookingDto);
        Task<BookingDto> GetBooking(Guid id);
        Task<List<BookingDto>> GetBookings();
        Task<BookingResponseDto> UpdateBookingStatus(Guid id, string status);
    }
}
