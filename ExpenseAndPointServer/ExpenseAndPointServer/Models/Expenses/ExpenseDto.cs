namespace ExpenseAndPointServer.Models.Expenses
{
    /// <summary>
    /// Класс расхода для работы с веб
    /// </summary>
    public class ExpenseDto
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
        /// Идентификтор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Преобразование класса ExpenseDto в Expense
        /// </summary>
        /// <returns>Класс Expense для взаимодействия с БД</returns>
        public Expense ToExpenseMap()
        {
            return new Expense
            {
                Id = Id,
                Amount = Amount,
                DateTime = DateTime,
                CategoryId = CategoryId,
                UserId = UserId
            };
        }
    }
}
