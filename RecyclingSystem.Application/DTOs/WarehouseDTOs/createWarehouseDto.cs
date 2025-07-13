using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.WarehouseDTOs
{
    public class createWarehouseDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "Name cannot exceed 20 characters.")]
        public string Name {  get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Name must be at least 3 characters.")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 20 characters.")]
        public string Location { get; set; }
        public string ManagerEmail { get; set; }
    }
}
