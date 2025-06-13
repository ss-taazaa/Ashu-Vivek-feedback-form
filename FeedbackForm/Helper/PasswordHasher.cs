using System.Security.Cryptography;

namespace FeedbackForm.Helper
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            // Generate a 128-bit salt
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32); // 256-bit hash

            return Convert.ToBase64String(salt.Concat(hash).ToArray());
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = hashBytes.Take(16).ToArray();
            byte[] storedSubHash = hashBytes.Skip(16).ToArray();

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            byte[] computedHash = pbkdf2.GetBytes(32);

            return storedSubHash.SequenceEqual(computedHash);
        }
    }
}
