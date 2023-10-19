using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseAndPointServer.Models
{
    /// <summary>
    /// Модель пользователя из БД
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        [MinLength(4)]
        public string Name { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        [MinLength(8)]
        public string Password { get; set; }
        /// <summary>
        /// Коллекция расходов
        /// </summary>
        public ICollection<Expense>? Expenses { get; set; }
        /// <summary>
        /// Коллекция категорий
        /// </summary>
        public ICollection<Category>? Categories { get; set; }
        
        /// <summary>
        /// Преобразование класса User в UserOutputDto
        /// </summary>
        /// <returns>Класс UserOutputDto для передачи на веб</returns>
        public UserOutputDto ToUserOutputDtoMap()
        {
            return new UserOutputDto
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
