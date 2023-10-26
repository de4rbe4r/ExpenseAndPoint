namespace ExpenseAndPointServer.Models.Expenses
{
    /// <summary>
    /// Класс для работы с входными данными для получения расхода по идентификатору пользователя, идентификатору категории и периоду
    /// </summary>
    public class ExpenseByUserIdAndCategoryIdAndPeriodRequest
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
        /// Дата начала
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        /// Дата конца
        /// </summary>
        public DateTime DateEnd { get; set; }
    }
}
