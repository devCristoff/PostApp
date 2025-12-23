using System.ComponentModel.DataAnnotations;

namespace PostApp.Core.Application.ViewModels.Users
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "You must enter a username")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "You must enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
