namespace ExpenseAndPointServer.Models.Expenses
{
    /// <summary>
    /// Класс для работы с входными данными для получения расхода по идентификатору пользователя и дате
    /// </summary>
    public class ExpenseByUserIdAndDateRequest
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }
    }
}
