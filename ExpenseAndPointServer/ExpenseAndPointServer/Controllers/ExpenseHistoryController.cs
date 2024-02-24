using ExpenseAndPoint.Data;
using ExpenseAndPointServer.ErrorLogging;
using ExpenseAndPointServer.Models.Expenses;
using ExpenseAndPointServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseAndPointServer.Controllers
{
    /// <summary>
    /// Контроллер для работы с историей расходов
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpenseHistoryController : ControllerBase
    {
        /// <summary>
        /// Сервис для работы с моделями расходов
        /// </summary>
        private ExpenseHistoryService expenseHistoryService;

        /// <summary>
        /// Логирование ошибок
        /// </summary>
        private readonly ErrorLogger _errorLogger = new ErrorLogger();

        /// <summary>
        /// Конструктор создания контроллера
        /// </summary>
        /// <param name="context">Контекст для работы с БД</param>
        public ExpenseHistoryController(AppDbContext context)
        {
            expenseHistoryService = new ExpenseHistoryService(context);
        }

        /// <summary>
        /// Получение списка истории расходов за последние 30 дней по идентификатору пользователя
        /// </summary>
        /// <param name="id">Идентификтаор пользователя</param>
        /// <returns>Список истории расходов</returns>
        /// <response code="200">Найденный список истории расходов</response>
        /// <response code="204">Список историй расходов пуст</response>
        /// <response code="500">Ошибка при поиске истории расходов</response> 
        [HttpGet("ByUserId/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ExpenseHistory>>> GetLast30DaysExpensesHistoryByUserId(int id)
        {
            try
            {
                var expenseList = await expenseHistoryService.GetLast30DaysExpenseHistoriesByUserId(id);
                var expenseDtoList = expenseList.Select(e => e.ToExpenseHistoryDtoMap());
                if (expenseList.Count() == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(expenseDtoList);
                }
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    "Id: " + id,
                    ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
