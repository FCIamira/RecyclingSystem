using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.UserInfDTOs
{
    public class UserInfoDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
