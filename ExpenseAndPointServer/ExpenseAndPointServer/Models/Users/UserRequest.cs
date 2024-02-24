using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseAndPointServer.Models.Users
{
    /// <summary>
    /// Класс пользователя приходящий с веб
    /// </summary>
    public class UserRequest
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string? Password { get; set; }
        
        /// <summary>
        /// Пароль
        /// </summary>
        public string? OldPassword { get; set; }

        /// <summary>
        /// Преобразование класса UserOtputDto в класс User
        /// </summary>
        /// <returns>Класс User для взаимодействия с БД</returns>
        public User ToUserMap()
        {
            return new User
            {
                Id = Id,
                Name = Name,
                Password = Password
            };
        }
    }
}
