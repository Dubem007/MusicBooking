using Microsoft.EntityFrameworkCore;
using MusicBooking.Domain.Entites;
using MusicBooking.Infrastructure.Data;
using MusicBooking.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Infrastructure.Repository
{
    public class UserRepository: Repository<AppUser>, IUserRepository
    {
        public UserRepository(MusicBookingDbContext context) : base(context) { }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            var users =  await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            return users;
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            var user =  await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
            return user;
        }

        public async Task<AppUser> GetUserWithDetailsAsync(Guid userId)
        {
            var user =  await _context.Users
                .Include(u => u.Artist)
                .FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }
    }
}
