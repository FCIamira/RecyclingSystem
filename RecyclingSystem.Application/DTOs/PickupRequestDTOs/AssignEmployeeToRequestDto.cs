using RecyclingSystem.Application.Feature.PickupRequest.Queries;
using RecyclingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.PickupRequestDTOs
{
    public class AssignEmployeeToRequestDto
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        //[Required]
      //  public string Status { get; set; }


    }
}
