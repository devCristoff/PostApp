namespace PostApp.Core.Application.DTOs.Account
{
    public class UpdateProfileRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
