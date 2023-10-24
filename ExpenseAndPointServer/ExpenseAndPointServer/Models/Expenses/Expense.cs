using ExpenseAndPointServer.Models.Users;
using ExpenseAndPointServer.Models.Categories;

namespace ExpenseAndPointServer.Models.Expenses
{
    /// <summary>
    /// Модель расходов из БД
    /// </summary>
    public class Expense
    {
        /// <summary>
        /// Идентификатор расхода
        /// </summary>
        public int Id { get; set; } 

        /// <summary>
        /// Сумма расхода
        /// </summary>
        public float Amount { get; set; }

        /// <summary>
        /// Дата и время
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Идентификатор категории
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Категория
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Преобразование класса Expense в ExpenseDto
        /// </summary>
        /// <returns>Класс ExpenseDto для взаимодействия с веб</returns>
        public ExpenseDto ToExpenseDtoMap()
        {
            return new ExpenseDto
            {
                Amount = Amount,
                DateTime = DateTime,
                CategoryId = CategoryId,
                UserId = UserId
            };
        }
    }
}
