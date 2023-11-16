using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace ExpenseAndPointServer.Services.Cryptographer
{
    /// <summary>
    /// Простой шифровщик пароля
    /// </summary>
    public class md5CryptographerService : ICryptographer
    {
        /// <inheritdoc/>
        public string Encrypt(string str)
        {
            // Генерация 128-битного salt, используя последовательность
            // криптографически надежных случайных байтов
            byte[] salt = new byte[str.Length];

            // Получение 256-байтного ключа используя HMACSHA256 со 100 000 итерациями

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: str,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
