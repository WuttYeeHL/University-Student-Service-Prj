using Microsoft.AspNetCore.Identity;
using AuthService.Model;

namespace AuthService.Services
{
    public class PasswordService
    {
        private readonly PasswordHasher<User> _hasher = new();

        public string HashPassword(User user, string password)
        {
            return _hasher.HashPassword(user, password);
        }

        public bool VerifyPassword(User user, string hashedPassword, string inputPassword)
        {
            var result = _hasher.VerifyHashedPassword(user, hashedPassword, inputPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
