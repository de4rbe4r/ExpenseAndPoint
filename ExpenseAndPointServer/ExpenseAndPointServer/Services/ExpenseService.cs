using ExpenseAndPoint.Data;
using ExpenseAndPointServer.Models.Expenses;
using Microsoft.EntityFrameworkCore;

namespace ExpenseAndPointServer.Services
{
    /// <summary>
    /// Сервис работы с расходами
    /// </summary>
    public class ExpenseService
    {
        /// <summary>
        /// Контекс для работы с БД
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Конструктор ExpenseService
        /// </summary>
        /// <param name="context">Контекст для работы с БД</param>
        public ExpenseService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Добавление расхода
        /// </summary>
        /// <param name="expense">Модель расхода для работы с БД</param>
        /// <returns>Поток с созданным расходом Task-Expense</returns>
        /// <exception cref="Exception">Ошибка связанный с отсутсвием пользователя/категории с указанным идентификатором или с некорректной датой</exception>
        public async Task<Expense> AddExpense(Expense expense)
        {
            if (_context.Users.FirstOrDefault(u => u.Id == expense.UserId) == null)
                throw new Exception($"Пользователя с идентификатором {expense.UserId} не существует");
            if (_context.Categories.FirstOrDefault(c => c.Id == expense.CategoryId) == null)
                throw new Exception($"Категории с идентификатором {expense.CategoryId} не существует");
            if (expense.DateTime > DateTime.Now)
                throw new Exception($"Нельзя добавить расход с указанием даты из будущего ({expense.DateTime.ToString("yyyyMMddHHmmss")})");
            if (expense.Amount < 0)
                throw new Exception($"Нельзя добавить расход с отрицательной суммой {expense.Amount}");

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }
      
        /// <summary>
        /// Получение списка расходов по идентификатору пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Отдельны поток с коллекцияей расходов</returns>
        /// <exception cref="Exception">Ошибка связанная с отсутствием пользователя с указанным идентификтаором</exception>
        public async Task<IEnumerable<Expense>> GetExpensesByUserId(int userId)
        {
            if (_context.Users.FirstOrDefault(u => u.Id == userId) == null)
                throw new Exception($"Пользователя с идентификатором {userId} не существует");
            return await _context.Expenses.Where(e => e.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Получение списка раходов по идентификатору пользователя и дате
        /// </summary>
        /// <param name="userId">Идентификтор пользователя</param>
        /// <param name="date">Дата расхода</param>
        /// <returns></returns>
        /// <exception cref="Exception">Ошибка связанный с отсутсвием пользователя с указанным идентификатором или с некорректной датой</exception>
        public async Task<IEnumerable<Expense>> GetExpenseByUserIdAndDate(int userId, DateTime date)
        {
            if (date > DateTime.Now)
                throw new Exception($"Не существует расходов с датой из будущего ({date.ToString("yyyyMMddHHmmss")})");
            if (_context.Users.FirstOrDefault(u => u.Id == userId) == null)
                throw new Exception($"Пользователя с идентификатором {userId} не существует");
            return await _context.Expenses.Where(e => e.UserId == userId && e.DateTime.Date == date.Date).ToListAsync();
        }
    }
}
