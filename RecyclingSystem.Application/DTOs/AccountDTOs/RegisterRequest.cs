using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.AccountDTOs
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "First Name Is Required")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Last Name Is Required")]
        public string LastName { get; set; } = null!;
        //[Required(ErrorMessage = "Full Name Is Required")]
        //public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email Address Is Required")]
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;
        public string? Address { get; set; }
        [Required(ErrorMessage = "Phone Number Is Required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; } = null!;
        ////[Required(ErrorMessage = "Phone Number Is Required")]
        ////[RegularExpression(@"^\+20\d{10}$", ErrorMessage = "Phone number must start with +20 and be followed by 10 digits")]
        ////public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]

        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirmation Password Is Required")]
        [Compare("Password", ErrorMessage = "Confirmation Password Not Match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        //[Required(ErrorMessage = "User Name Is Required")]
        //[MinLength(6, ErrorMessage = "Must More Than 6 Charachters")]
        //public string UserName { get; set; } = null!;
    }
}
