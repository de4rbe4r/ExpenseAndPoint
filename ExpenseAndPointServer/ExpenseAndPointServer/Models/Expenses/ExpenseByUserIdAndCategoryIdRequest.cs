namespace ExpenseAndPointServer.Models.Expenses
{
    /// <summary>
    /// Класс для работы с входными данными для получения расхода по идентификатору пользователя и идентификатору категории
    /// </summary>
    public class ExpenseByUserIdAndCategoryIdRequest
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Идентификатор категории
        /// </summary>
        public int CategoryId { get; set; }
    }
}
