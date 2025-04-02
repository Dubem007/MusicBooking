using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBooking.Domain
{
    public class ClaimTypeHelper
    {
        public static string UserId { get; set; } = "UserId";
        public static string Email { get; set; } = "Email";
        public static string PhoneNumber { get; set; } = "PhoneNumber";
        public static string IsOrganizer { get; set; } = "IsOrganizer";
    }
}
