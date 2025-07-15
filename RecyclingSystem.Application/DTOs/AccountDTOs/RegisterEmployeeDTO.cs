using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.AccountDTOs
{
    public class RegisterEmployeeDTO
    {
     
        [Required(ErrorMessage = "First Name Is Required")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Last Name Is Required")]
        public string LastName { get; set; } = null!;
           

        [Required(ErrorMessage = "Email Address Is Required")]
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;
        public string? Address { get; set; }
        [Required(ErrorMessage = "Phone Number Is Required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; } = null!;
            

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]

        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirmation Password Is Required")]
        [Compare("Password", ErrorMessage = "Confirmation Password Not Match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
        public string WarehouseName { get; set; }
        public int WarehouseId { get; set; }
        
    }
}
