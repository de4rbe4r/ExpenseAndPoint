using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace ExpenseAndPointServer.Services.Cryptographer
{
    public class md5CryptographerService : ICryptographer
    {
        public string Encrypt(string str)
        {
            // Генерация 128-битного salt, используя последовательность
            // криптографически надежных случайных байтов
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

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
