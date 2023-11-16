using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ExpenseAndPointServer.Authentication
{
    /// <summary>
    /// Опции аутенфикации
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// Издатель токена
        /// </summary>
        public const string ISSUER = "localhost:7184"; 

        /// <summary>
        /// Потребитель токена
        /// </summary>
        public const string AUDINCE = "localhost:5173";

        // Минимальная длина строки, чтобы сработало шифрование - 17 символов!
        private const string KEY = "симметричныйКлючШифрования_CJIo}|{HbIu"; // Ключ для шифрации

        /// <summary>
        /// Время жизни токена в минутах
        /// </summary>
        public const int LIFETIME = 43200;

        /// <summary>
        /// Получение симметричного ключа шифрования
        /// </summary>
        /// <returns></returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
