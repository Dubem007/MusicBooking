using MusicBooking.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Application.Util
{
    public static class HashingUtility
    {
        public static string HashString(string data)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] hashValue = sha512.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToHexString(hashValue);
            }

        }
        public static PasswordHashDetails HashPassword(string data, AppSettings config, string salt = null)
        {
            if (string.IsNullOrEmpty(salt))
            {
                var generator = RandomNumberGenerator.Create();
                byte[] saltByte = new byte[16];
                generator.GetNonZeroBytes(saltByte);
                salt = Encoding.Default.GetString(saltByte);
            }
            var hashData = data;
            for (int i = 0; i < config.PasswordHashIteration; i++)
            {
                hashData = HashString($"{salt}{hashData}{config.PepperKey}");
            }

            return new PasswordHashDetails
            {
                Salt = salt,
                HashedValue = hashData
            };
        }
    }
}
