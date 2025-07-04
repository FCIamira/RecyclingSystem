using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Domain.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression("Customer|Employee|Admin|Manager", ErrorMessage = "Invalid role")]
        public string Role { get; set; }

        [Range(0, int.MaxValue)]
        public int TotalPoints { get; set; } = 0;

        public string? Address { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<PointsHistory>? PointsHistory { get; set; }
        public virtual ICollection<EmployeeWarehouseHistory>? Employees { get; set; }
    }
}
