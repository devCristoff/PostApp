using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PostApp.Core.Application.ViewModels.Users
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "You must enter a name")]
        [DataType(DataType.Text)] 
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You must enter a lastname")]
        [DataType(DataType.Text)] 
        public string LastName { get; set; }

        [Required(ErrorMessage = "You must enter a email")]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must enter a telephone")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\((809|829|849)\) \d{3}-\d{4}$", ErrorMessage = "The phone must have the following format: (809|829|849) ###-####.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "You must enter the username")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "You must enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords must match")]
        [Required(ErrorMessage = "You must enter a confirm password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }
        public string? ImageUrl { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
