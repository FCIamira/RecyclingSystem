using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.WharehosingInventoryDTOs
{
    public class AddWharehosingInventoryDto
    {
        [Required]
        public int MaterialId { get; set; }
        public int Quantity { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
