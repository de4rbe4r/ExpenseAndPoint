using ExpenseAndPoint.Data;
using ExpenseAndPointServer.Models.Expenses;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

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
        /// Сервис для работы с историей расходов
        /// </summary>
        private readonly ExpenseHistoryService _expenseHistoryService;

        /// <summary>
        /// Конструктор ExpenseService
        /// </summary>
        /// <param name="context">Контекст для работы с БД</param>
        public ExpenseService(AppDbContext context)
        {
            _context = context;
            _expenseHistoryService = new ExpenseHistoryService(context);
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
            await _expenseHistoryService.AddExpenseHistory(ActionType.Create, expense);
            return expense;
        }
      
        /// <summary>
        /// Получение списка расходов по идентификатору пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Коллекция расходов</returns>
        /// <exception cref="Exception">Ошибка связанная с отсутствием пользователя с указанным идентификтаором</exception>
        public async Task<IEnumerable<Expense>> GetExpensesByUserId(int userId)
        {
            if (_context.Users.FirstOrDefault(u => u.Id == userId) == null)
                throw new Exception($"Пользователя с идентификатором {userId} не существует");
            return await _context.Expenses.Where(e => e.UserId == userId).OrderBy(e => e.DateTime.TimeOfDay).ToListAsync();
        }

        /// <summary>
        /// Получение расхода по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор расхода</param>
        /// <returns>Найденный расход</returns>
        public async Task<Expense> GetExpensesById(int id)
        {
            return await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// Получение списка раходов по идентификатору пользователя и дате
        /// </summary>
        /// <param name="userId">Идентификтор пользователя</param>
        /// <param name="date">Дата расхода</param>
        /// <returns>Коллекция расходов</returns>
        /// <exception cref="Exception">Ошибка связанный с отсутсвием пользователя с указанным идентификатором или с некорректной датой</exception>
        public async Task<IEnumerable<Expense>> GetExpenseByUserIdAndDate(int userId, DateTime date)
        {
            if (date > DateTime.Now)
                throw new Exception($"Не существует расходов с датой из будущего ({date.ToString("yyyyMMddHHmmss")})");
            if (_context.Users.FirstOrDefault(u => u.Id == userId) == null)
                throw new Exception($"Пользователя с идентификатором {userId} не существует");
            return await _context.Expenses.Where(e => e.UserId == userId && e.DateTime.Date == date.Date).ToListAsync();
        }

        /// <summary>
        /// Получение списка раходов по идентификатору пользователя и периоду
        /// </summary>
        /// <param name="userId">Идентификтор пользователя</param>
        /// <param name="dateStart">Дата начала периода</param>
        /// <param name="dateEnd">Дата конца периода</param>
        /// <returns>Коллекция расходов</returns>
        /// <exception cref="Exception">Ошибка связанный с отсутсвием пользователя с указанным идентификтором</exception>
        public async Task<IEnumerable<Expense>> GetExpenseByUserIdAndPeriod(int userId, DateTime dateStart, DateTime dateEnd)
        {
            if (_context.Users.FirstOrDefault(u => u.Id == userId) == null)
                throw new Exception($"Пользователя с идентификатором {userId} не существует");
            if (dateStart > dateEnd)
            {
                var temp = dateStart;
                dateStart = dateEnd;
                dateEnd = temp;
            }
            return await _context.Expenses.Where(e => e.UserId == userId 
                                                && e.DateTime.Date > dateStart.Date 
                                                && e.DateTime.Date < dateEnd.Date).ToListAsync();
        }

        /// <summary>
        /// Получение списка расходов по идентификатору пользователя и идентификатору категории
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="categoryId">Идентификатор категории</param>
        /// <returns>Коллекция расходов</returns>
        /// <exception cref="Exception">Ошибка связанная с отсутствием пользователя или категории с указанным идентификтором</exception>
        public async Task<IEnumerable<Expense>> GetExpensesByUserIdAndCategoryId(int userId, int categoryId)
        {
            if (_context.Users.FirstOrDefault(u => u.Id == userId) == null)
                throw new Exception($"Пользователя с идентификатором {userId} не существует");
            if (_context.Categories.FirstOrDefault(c => c.Id == categoryId) == null)
                throw new Exception($"Категории с идентификатором {categoryId} не существует");
            return await _context.Expenses.Where(e => e.UserId == userId && e.CategoryId == categoryId).ToListAsync();
        }

        /// <summary>
        /// Получение списка расходов по идентификатору пользователя, идентификатору категории и дате
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="categoryId">Идентификатор категории</param>
        /// <param name="date">Дата</param>
        /// <returns>Коллекция расходов</returns>
        /// <exception cref="Exception">Ошибка связанная с отсутствием пользователя или категории с указанным идентификтором, неверно указанной датой</exception>
        public async Task<IEnumerable<Expense>> GetExpensesByUserIdAndCategoryIdAndDate(int userId, int categoryId, DateTime date)
        {
            if (_context.Users.FirstOrDefault(u => u.Id == userId) == null)
                throw new Exception($"Пользователя с идентификатором {userId} не существует");
            if (_context.Categories.FirstOrDefault(c => c.Id == categoryId) == null)
                throw new Exception($"Категории с идентификатором {categoryId} не существует");
            if (date > DateTime.Now)
                throw new Exception($"Не существует расходов с датой из будущего ({date.ToString("yyyyMMddHHmmss")})");
            return await _context.Expenses.Where(e => e.UserId == userId && e.CategoryId == categoryId 
                                                    && e.DateTime.Date == date.Date).ToListAsync();
        }

        /// <summary>
        /// Получение списка расходов по идентификатору пользователя, идентификатору категории и периоду
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="categoryId">Идентификатор категории</param>
        /// <param name="dateStart">Дата начала</param>
        /// <param name="dateEnd">Дата конца</param>
        /// <returns>Коллекцияе расходов</returns>
        /// <exception cref="Exception">Ошибка связанная с отсутствием пользователя или категории с указанным идентификтором, неверно указанной датой</exception>
        public async Task<IEnumerable<Expense>> GetExpensesByUserIdAndCategoryIdAndPeriod(int userId, int categoryId, DateTime dateStart, DateTime dateEnd)
        {
            if (_context.Users.FirstOrDefault(u => u.Id == userId) == null)
                throw new Exception($"Пользователя с идентификатором {userId} не существует");
            if (_context.Categories.FirstOrDefault(c => c.Id == categoryId) == null)
                throw new Exception($"Категории с идентификатором {categoryId} не существует");
            if (dateStart > dateEnd)
            {
                var temp = dateStart;
                dateStart = dateEnd;
                dateEnd = temp;
            }
            return await _context.Expenses.Where(e => e.UserId == userId && e.CategoryId == categoryId
                                                    && e.DateTime.Date > dateStart.Date
                                                    && e.DateTime.Date < dateEnd.Date).ToListAsync();
        }

        /// <summary>
        /// Изменение расхода
        /// </summary>
        /// <param name="id">Идентификатор расхода</param>
        /// <param name="expense">Модель расхода для работы с БД</param>
        /// <returns>Измененная категория</returns>
        /// <exception cref="Exception">Ошибка в передаче данных</exception>
        public async Task<Expense> EditExpense(int id, Expense expense)
        {
            if (id != expense.Id) throw new Exception("Переданные Id и расход не совпадают! Проверьте опарвляемые данные");
            Expense editedExpense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id);
            _context.Entry(expense).Property(e => e.DateTime).IsModified = true;
            _context.Entry(expense).Property(e => e.Amount).IsModified = true;
            _context.Entry(expense).Property(e => e.CategoryId).IsModified = true;
            await _context.SaveChangesAsync();
            await _expenseHistoryService.AddExpenseHistory(ActionType.Change, expense, editedExpense);
            return await GetExpensesById(id);
        }

        /// <summary>
        /// Удаление расхода по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор расхода</param>
        public async Task DeleteExpenseById(int id)
        {
            var expense = await GetExpensesById(id);
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            await _expenseHistoryService.AddExpenseHistory(ActionType.Delete, expense);
        }
    }
}
