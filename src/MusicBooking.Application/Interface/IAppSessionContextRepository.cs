using MusicBooking.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Application.Interface
{
    public interface IAppSessionContextRepository
    {
        Task<UserDto> GetUserDetails();
    }
}
