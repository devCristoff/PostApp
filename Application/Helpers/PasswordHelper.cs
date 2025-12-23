namespace PostApp.Core.Application.Helpers
{
    public static class PasswordHelper
    {
        public static async Task<string> UpdatePassword()
        {
            int length = 15;
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
            var random = new Random();
            string password = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return password;
        }
    }
}
