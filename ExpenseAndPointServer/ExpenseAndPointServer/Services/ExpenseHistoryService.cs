using ExpenseAndPoint.Data;
using ExpenseAndPointServer.Models.Expenses;
using Microsoft.EntityFrameworkCore;

namespace ExpenseAndPointServer.Services
{
    /// <summary>
    /// Сервис работы с историей расходов
    /// </summary>
    public class ExpenseHistoryService
    {
        /// <summary>
        /// Контекст для работы с БД
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Конструктор CategoryService
        /// </summary>
        /// <param name="context">Контекст для работы с БД</param>
        public ExpenseHistoryService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Добавление истории расходов
        /// </summary>
        /// <param name="expenseHistory"></param>
        /// <returns>Добавленная история расходов</returns>
        public async Task<ExpenseHistory> AddExpenseHistory(ActionType action, Expense newExpense, Expense oldExpense = null)
        {
            ExpenseHistory history = new ExpenseHistory();
            switch (action)
            {
                case ActionType.Create:
                    history.ActionType = action;
                    history.DateCreated = DateTime.Now;
                    history.UserId = newExpense.UserId;
                    history.NewAmount = newExpense.Amount;
                    history.NewCategoryTitle = 
                             _context.Categories.FirstOrDefault(c => c.Id == newExpense.CategoryId).Title;
                    history.NewDateTime = newExpense.DateTime;
                    break;
                case ActionType.Delete:
                    history.ActionType = action;
                    history.DateCreated = DateTime.Now;
                    history.UserId = newExpense.UserId;
                    history.NewAmount = newExpense.Amount;
                    history.NewCategoryTitle =
                            _context.Categories.FirstOrDefault(c => c.Id == newExpense.CategoryId).Title;
                    history.NewDateTime = newExpense.DateTime;
                    break;
                case ActionType.Change:
                    history.ActionType = action;
                    history.DateCreated = DateTime.Now;
                    history.UserId = newExpense.UserId;
                    history.NewAmount = newExpense.Amount;
                    history.NewCategoryTitle =
                            _context.Categories.FirstOrDefault(c => c.Id == newExpense.CategoryId).Title;
                    history.NewDateTime = newExpense.DateTime;
                    history.OldAmount = oldExpense.Amount;
                    history.OldCategoryTitle =
                            _context.Categories.FirstOrDefault(c => c.Id == newExpense.CategoryId).Title;
                    history.OldDateTime = oldExpense.DateTime;
                    break;
            }
            _context.ExpenseHistories.Add(history);
            await _context.SaveChangesAsync();
            return history;
        }

        /// <summary>
        /// Получить список истории расходо за последнии 30 дней по идентификатору пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Коллекция истории расходов</returns>
        /// <exception cref="Exception">Ошибка связанный с отсутсвием пользователя с указанным идентификтором</exception>
        public async Task<IEnumerable<ExpenseHistory>> GetLast30DaysExpenseHistoriesByUserId(int userId)
        {
            if (_context.Users.FirstOrDefault(u => u.Id == userId) == null)
                throw new Exception($"Пользователя с идентификатором {userId} не существует");
            return await _context.ExpenseHistories.Where(e => e.UserId == userId 
                                && e.DateCreated < DateTime.Now 
                                || e.DateCreated > DateTime.Now.AddDays(-30)).ToListAsync();
        }
    }
}
