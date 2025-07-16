using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.CustomerInfoDTOs
{
    public class CancelRequestResultDto
    {
        public string Message { get; set; } = string.Empty;
        public bool ShouldDeductPoints { get; set; }
    }

}
