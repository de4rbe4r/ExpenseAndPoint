using ExpenseAndPointServer.Models.Users;
using ExpenseAndPointServer.Models.Expenses;


namespace ExpenseAndPointServer.Models.Categories

{
    /// <summary>
    /// Модель категории из БД
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Идентификатор категории
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Коллекция расходов
        /// </summary>
        public ICollection<Expense>? Expenses { get; set; }

        /// <summary>
        /// Преобразование класса Category в CategoryDto
        /// </summary>
        /// <returns>Класс CategoryDto для передачи на веб</returns>
        public CategoryDto ToCategoryDtoMap()
        {
            return new CategoryDto
            {
                Id = Id,
                UserId = UserId,
                Title = Title
            };
        }  
    }
}
