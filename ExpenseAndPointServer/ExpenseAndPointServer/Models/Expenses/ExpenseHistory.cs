using ExpenseAndPointServer.Models.Categories;
using ExpenseAndPointServer.Models.Users;

namespace ExpenseAndPointServer.Models.Expenses
{
    public class ExpenseHistory
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
        /// Пользователь
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Действие
        /// </summary>
        public ActionType ActionType { get; set; }

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

        public ExpenseHistoryDto ToExpenseHistoryDtoMap()
        {
            ExpenseHistoryDto expenseHistoryDto = new ExpenseHistoryDto();
            switch (ActionType)
            {
                case ActionType.Create:
                    expenseHistoryDto.Action = "Создал";
                    break;
                case ActionType.Delete:
                    expenseHistoryDto.Action = "Удалил";
                    break;
                case ActionType.Change:
                    expenseHistoryDto.Action = "Изменил";
                    break;
            }
            expenseHistoryDto.Id = Id;
            expenseHistoryDto.DateCreated = DateCreated;
            expenseHistoryDto.NewCategoryTitle = NewCategoryTitle;
            expenseHistoryDto.OldCategoryTitle = OldCategoryTitle;
            expenseHistoryDto.OldDateTime = OldDateTime;
            expenseHistoryDto.NewDateTime = NewDateTime;
            expenseHistoryDto.UserId = UserId;
            expenseHistoryDto.NewAmount = NewAmount;
            expenseHistoryDto.OldAmount = OldAmount;
            return expenseHistoryDto;
        }
    }
}
