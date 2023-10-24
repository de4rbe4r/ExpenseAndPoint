using ExpenseAndPoint.Data;
using ExpenseAndPointServer.Models.Expenses;
using ExpenseAndPointServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseAndPointServer.Controllers
{
    /// <summary>
    /// Контроллер для работы с расходами
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        /// <summary>
        /// Сервис для работы с моделями расходов
        /// </summary>
        private ExpenseService expenseService;

        /// <summary>
        /// Конструктор создания контроллера
        /// </summary>
        /// <param name="context">Контекст для работы с БД</param>
        public ExpenseController(AppDbContext context)
        {
            expenseService = new ExpenseService(context);
        }

        /// <summary>
        /// Добавление расхода
        /// </summary>
        /// <param name="expenseDto"></param>
        /// <returns>Добавленный расход</returns>
        /// <response code="200">Созданный расход</response>
        /// <response code="500">Ошибка при создании расхода</response> 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ExpenseDto>> PostExpense(ExpenseDto expenseDto)
        {
            try
            {
                var expense = await expenseService.AddExpense(expenseDto.ToExpenseMap());
                return Ok(expense.ToExpenseDtoMap());
            } catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Получение списка расходов по идентификатору пользователя
        /// </summary>
        /// <param name="id">Идентификтаор пользователя</param>
        /// <returns>Список категорий</returns>
        /// <response code="200">Найденный список расходов</response>
        /// <response code="204">Список расходов пуст</response>
        /// <response code="500">Ошибка при поиске расходов</response> 
        [HttpGet("ByUserId/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByUserId(int id)
        {
            try
            {
                var expenseList = await expenseService.GetExpensesByUserId(id);
                var expenseDtoList = expenseList.Select(e => e.ToExpenseDtoMap());
                if (expenseList.Count() == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(expenseDtoList);
                }
            } catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Получение списка расходов по идентификатору пользователя
        /// </summary>
        /// <param name="expenseInput"> Класс для работы с входными данными для получения расхода по идентификатору пользователя и дате</param>
        /// <returns>Список категорий</returns>
        /// <response code="200">Найденный список расходов</response>
        /// <response code="204">Список расходов пуст</response>
        /// <response code="500">Ошибка при поиске расходов</response> 
        [HttpPost("ByUserIdAndDate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByUserIdAndDate(ExpenseByUserIdAndDateDto expenseInput)
        {
            try
            {
                var expenseList = await expenseService.GetExpenseByUserIdAndDate(expenseInput.UserId, expenseInput.date);
                var expenseDtoList = expenseList.Select(e => e.ToExpenseDtoMap());
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
                return Problem(ex.Message);
            }
        }
    }
}
