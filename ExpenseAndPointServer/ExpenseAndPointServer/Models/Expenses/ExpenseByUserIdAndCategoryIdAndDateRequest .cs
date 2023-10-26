namespace ExpenseAndPointServer.Models.Expenses
{
    /// <summary>
    /// Класс для работы с входными данными для получения расхода по идентификатору пользователя, идентификатору категории и дате
    /// </summary>
    public class ExpenseByUserIdAndCategoryIdAndDateRequest
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Идентификатор категории
        /// </summary>
        public int CategoryId { get; set; }
        
        /// <summary>
        /// Дата расхода
        /// </summary>
        public DateTime Date { get; set; }
    }
}
