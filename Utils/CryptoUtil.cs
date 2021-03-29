using System;
using System.Security.Cryptography;
using System.Text;

namespace PersonalCrm
{
    public class CryptoUtil
    {
        public static string GetSaltedPasswordHash(string password, string salt)
        {
            byte[] pwdBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            byte[] saltedPassword = new byte[pwdBytes.Length + saltBytes.Length];

            Buffer.BlockCopy(pwdBytes, 0, saltedPassword, 0, pwdBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, saltedPassword, pwdBytes.Length, saltBytes.Length);

            SHA1 sha = SHA1.Create();

            return Convert.ToBase64String(sha.ComputeHash(saltedPassword));
        }

        public static string GenerateSalt()
        {
            var random = new RNGCryptoServiceProvider();
            var max_length = 32;
            byte[] salt = new byte[max_length];
            random.GetNonZeroBytes(salt);
            return Convert.ToBase64String(salt);
        }
    }
}