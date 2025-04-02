using MusicBooking.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Infrastructure.Interface
{
    public interface IUserRepository : IRepository<AppUser>
    {
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<AppUser> GetUserWithDetailsAsync(Guid userId);
    }
}
