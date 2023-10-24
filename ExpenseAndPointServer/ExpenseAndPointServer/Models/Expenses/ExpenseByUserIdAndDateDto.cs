namespace ExpenseAndPointServer.Models.Expenses
{
    /// <summary>
    /// Класс для работы с входными данными для получения расхода по идентификатору пользователя и дате
    /// </summary>
    public class ExpenseByUserIdAndDateDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime date { get; set; }


    }
}
