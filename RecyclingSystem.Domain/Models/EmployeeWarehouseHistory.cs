using RecyclingSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Domain.Models
{
    public class EmployeeWarehouseHistory : BaseModel<int>
    {
        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [ForeignKey("Warehouse")]
        public int WarehouseId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime AssignedDate { get; set; }

        //Navigation Property
        public ApplicationUser? Employee { get; set; }
        public Warehouse? Warehouse { get; set; }
    }
}
