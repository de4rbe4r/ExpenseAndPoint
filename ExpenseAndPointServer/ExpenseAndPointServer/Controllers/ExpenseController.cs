using ExpenseAndPoint.Data;
using ExpenseAndPointServer.ErrorLogging;
using ExpenseAndPointServer.Models.Expenses;
using ExpenseAndPointServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ExpenseAndPointServer.Controllers
{
    /// <summary>
    /// Контроллер для работы с расходами
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        /// <summary>
        /// Сервис для работы с моделями расходов
        /// </summary>
        private ExpenseService expenseService;

        /// <summary>
        /// Логирование ошибок
        /// </summary>
        private readonly ErrorLogger _errorLogger = new ErrorLogger();

        /// <summary>
        /// Конструктор создания контроллера
        /// </summary>
        /// <param name="context">Контекст для работы с БД</param>
        public ExpenseController(AppDbContext context)
        {
            expenseService = new ExpenseService(context);
        }

        /// <summary>
        /// Получение списка расходов по идентификатору пользователя
        /// </summary>
        /// <param name="id">Идентификтаор пользователя</param>
        /// <returns>Список расходов</returns>
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
                return Ok(expenseDtoList);
            } catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    "Id: " + id,
                    ex.Message);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Получение списка расходов по идентификатору расходу
        /// </summary>
        /// <param name="id">Идентификтаор расхода</param>
        /// <returns>Расход</returns>
        /// <response code="200">Найденный расход</response>
        /// <response code="204">Расход не найден</response>
        /// <response code="500">Ошибка при поиске расхода</response> 
        [HttpGet("ById/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ExpenseDto>> GetExpensesById(int id)
        {
            try
            {
                var expense = await expenseService.GetExpensesById(id);
                return Ok(expense.ToExpenseDtoMap());
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    "Id:" + id,
                    ex.Message);
                return Problem(ex.Message);
            }
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
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    JsonSerializer.Serialize(expenseDto),
                    ex.Message);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Получение списка расходов по идентификатору пользователя и дате
        /// </summary>
        /// <param name="expenseInput"> Класс для работы с входными данными для получения расхода по идентификатору пользователя и дате</param>
        /// <returns>Список расходов</returns>
        /// <response code="200">Найденный список расходов</response>
        /// <response code="204">Список расходов пуст</response>
        /// <response code="500">Ошибка при поиске расходов</response> 
        [HttpPost("ByUserIdAndDate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByUserIdAndDate(ExpenseByUserIdAndDateRequest expenseInput)
        {
            try
            {
                var expenseList = await expenseService.GetExpenseByUserIdAndDate(expenseInput.UserId, expenseInput.Date);
                var expenseDtoList = expenseList.Select(e => e.ToExpenseDtoMap());
                    return Ok(expenseDtoList);
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    JsonSerializer.Serialize(expenseInput),
                    ex.Message);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Получение списка расходов по идентификатору пользователя и периоду
        /// </summary>
        /// <param name="expenseInput"> Класс для работы с входными данными для получения расхода по идентификатору пользователя и периоду</param>
        /// <returns>Список расходов</returns>
        /// <response code="200">Найденный список расходов</response>
        /// <response code="204">Список расходов пуст</response>
        /// <response code="500">Ошибка при поиске расходов</response> 
        [HttpPost("ByUserIdAndPeriod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByUserIdAndPeriod(ExpenseByUserIdAndPeriodRequest expenseInput)
        {
            try
            {
                var expenseList = await expenseService.GetExpenseByUserIdAndPeriod(expenseInput.UserId, expenseInput.DateStart, expenseInput.DateEnd);
                var expenseDtoList = expenseList.Select(e => e.ToExpenseDtoMap());
                return Ok(expenseDtoList);
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    JsonSerializer.Serialize(expenseInput),
                    ex.Message);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Получение списка расходов по идентификатору пользователя и идентификатору категории
        /// </summary>
        /// <param name="expenseInput"> Класс для работы с входными данными для получения расхода по идентификатору пользователя и идентификатору категории</param>
        /// <returns>Список расходов</returns>
        /// <response code="200">Найденный список расходов</response>
        /// <response code="204">Список расходов пуст</response>
        /// <response code="500">Ошибка при поиске расходов</response> 
        [HttpPost("ByUserIdAndCategoryId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByUserIdAndCategoryId(ExpenseByUserIdAndCategoryIdRequest expenseInput)
        {
            try
            {
                var expenseList = await expenseService.GetExpensesByUserIdAndCategoryId(expenseInput.UserId, expenseInput.CategoryId);
                var expenseDtoList = expenseList.Select(e => e.ToExpenseDtoMap());
                return Ok(expenseDtoList);
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    JsonSerializer.Serialize(expenseInput),
                    ex.Message);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Получение списка расходов по идентификатору пользователя, идентификтору категории и дате
        /// </summary>
        /// <param name="expenseInput"> Класс для работы с входными данными для получения расхода по идентификатору пользователя, идентификатору категории, дате</param>
        /// <returns>Список расходов</returns>
        /// <response code="200">Найденный список расходов</response>
        /// <response code="204">Список расходов пуст</response>
        /// <response code="500">Ошибка при поиске расходов</response> 
        [HttpPost("ByUserIdAndCategoryIdAndDate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByUserIdAndCategoryIdAndDate(ExpenseByUserIdAndCategoryIdAndDateRequest expenseInput)
        {
            try
            {
                var expenseList = await expenseService.GetExpensesByUserIdAndCategoryIdAndDate(expenseInput.UserId, expenseInput.CategoryId, expenseInput.Date);
                var expenseDtoList = expenseList.Select(e => e.ToExpenseDtoMap());
                return Ok(expenseDtoList);
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    JsonSerializer.Serialize(expenseInput),
                    ex.Message);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Получение списка расходов по идентификатору пользователя, идентификтору категории и периоду
        /// </summary>
        /// <param name="expenseInput"> Класс для работы с входными данными для получения расхода по идентификатору пользователя, идентификатору категории, периоду</param>
        /// <returns>Список расходов</returns>
        /// <response code="200">Найденный список расходов</response>
        /// <response code="204">Список расходов пуст</response>
        /// <response code="500">Ошибка при поиске расходов</response> 
        [HttpPost("ByUserIdAndCategoryIdAndPeriod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByUserIdAndCategoryIdAndPeriod(ExpenseByUserIdAndCategoryIdAndPeriodRequest expenseInput)
        {
            try
            {
                var expenseList = await expenseService.GetExpensesByUserIdAndCategoryIdAndPeriod(expenseInput.UserId, expenseInput.CategoryId, expenseInput.DateStart, expenseInput.DateEnd);
                var expenseDtoList = expenseList.Select(e => e.ToExpenseDtoMap());
                return Ok(expenseDtoList);
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    JsonSerializer.Serialize(expenseInput),
                    ex.Message);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Изменение расхода
        /// </summary>
        /// <param name="id">Идентификатор расхода</param>
        /// <param name="expenseDto">Класс расхода приходящий с веб</param>
        /// <returns>Измененный пользователь</returns>
        /// <response code="200">Измененная категория</response> 
        /// <response code="500">Ошибка при удалении категории</response> 
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ExpenseDto>> PutCategory(int id, ExpenseDto expenseDto)
        {
            Expense editedExpense;
            try
            {
                editedExpense = await expenseService.EditExpense(id, expenseDto.ToExpenseMap());
            } catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    JsonSerializer.Serialize(expenseDto),
                    ex.Message);
                return Problem(ex.Message);
            }
            return Ok(editedExpense.ToExpenseDtoMap());
        }

        /// <summary>
        /// Удаление расхода по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор расхода</param>
        /// <response code="200">Расход удален</response> 
        /// <response code="500">Ошибка при удалении расход</response> 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                await expenseService.DeleteExpenseById(id);
                return Ok();
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
