﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.UserInfDTOs
{
    public class UserInfoDto
    {
        public int Id { get; set; }

        [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "Name cannot exceed 20 characters.")]
        public string? FullName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public string? Address { get; set; }

        [MaxLength(11, ErrorMessage = "Phone Number cannot exceed 11 number.")]
        public string? PhoneNumber { get; set; }

        [Required]
        [RegularExpression("Customer|Employee|Admin|Manager", ErrorMessage = "Invalid role")]
        public string Role { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int? TotalPoints { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
