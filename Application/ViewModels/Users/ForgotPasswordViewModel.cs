using System.ComponentModel.DataAnnotations;

namespace PostApp.Core.Application.ViewModels.Users
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "You must enter the username")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
