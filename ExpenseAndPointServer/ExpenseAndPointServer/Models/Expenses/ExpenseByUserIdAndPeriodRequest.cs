namespace ExpenseAndPointServer.Models.Expenses
{
    /// <summary>
    /// Класс для работы с входными данными для получения расхода по идентификатору пользователя и периоду
    /// </summary>
    public class ExpenseByUserIdAndPeriodRequest
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Дата начала периода
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        /// Дата конца периода
        /// </summary>
        public DateTime DateEnd { get; set; }
    }
}
