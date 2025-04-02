using MusicBooking.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Application.Interface
{
    public interface IAuthService
    {
        Task<UserDto> GetCurrentUser();
        Task<LoginViewModel> Login(LoginUserDto loginDto);
        Task<AuthResponseDto> Register(RegisterUserDto registerDto);
    }
}
