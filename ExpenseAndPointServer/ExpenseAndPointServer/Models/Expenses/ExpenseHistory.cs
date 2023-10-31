using ExpenseAndPointServer.Models.Categories;
using ExpenseAndPointServer.Models.Users;

namespace ExpenseAndPointServer.Models.Expenses
{
    public class ExpenseHistory
    {
        /// <summary>
        /// Дата создания записи
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Действие
        /// </summary>
        public string Action { get; set; }

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
    }
}
