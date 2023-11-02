using ExpenseAndPointServer.Models.Users;

namespace ExpenseAndPointServer.Models.Expenses
{
    public class ExpenseHistoryDto
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата создания записи
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Действие
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Новая сумма расхода 
        /// </summary>
        public float NewAmount { get; set; }

        /// <summary>
        /// Новые дата и время
        /// </summary>
        public DateTime NewDateTime { get; set; }

        /// <summary>
        /// Новый название категории
        /// </summary>
        public string NewCategoryTitle { get; set; }

        /// <summary>
        /// Старая сумма расхода 
        /// </summary>
        public float? OldAmount { get; set; }

        /// <summary>
        /// Старые дата и время
        /// </summary>
        public DateTime? OldDateTime { get; set; }

        /// <summary>
        /// Старый название категории
        /// </summary>
        public string? OldCategoryTitle { get; set; }
    }
}
