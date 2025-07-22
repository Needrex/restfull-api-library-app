using System;
using System.Security.Cryptography;
using System.Text;

namespace RestApiApp.Utils
{
    public class PasswordHasher
    {
        // Hash password
        public static string HashPassword(string password)
        {
            // Buat salt random
            byte[] salt = RandomNumberGenerator.GetBytes(16); // 128-bit salt

            // Derive key menggunakan PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32); // 256-bit hash

            // Gabungkan salt + hash, dan encode ke base64 untuk disimpan
            byte[] hashBytes = new byte[48];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
            Buffer.BlockCopy(hash, 0, hashBytes, 16, 32);

            return Convert.ToBase64String(hashBytes);
        }

        // Verifikasi password
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            hashedPassword = hashedPassword.Trim()
                                        .Replace("\n", "")
                                        .Replace("\r", "")
                                        .Replace(" ", "")
                                        .Replace("\u00A0", "");
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[16];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, 16);

            byte[] storedHash = new byte[32];
            Buffer.BlockCopy(hashBytes, 16, storedHash, 0, 32);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] computedHash = pbkdf2.GetBytes(32);

            // Bandingkan byte-by-byte
            return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
        }
    }
}