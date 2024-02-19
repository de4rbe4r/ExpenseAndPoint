using ExpenseAndPoint.Data;
using ExpenseAndPointServer.Services;
using ExpenseAndPointServer.Models.Categories;
using Microsoft.AspNetCore.Mvc;
using ExpenseAndPointServer.ErrorLogging;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseAndPointServer.Controllers
{
    /// <summary>
    /// Контроллер для работы с категориями
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        /// <summary>
        /// Сервис для работы с моделями категорий
        /// </summary>
        private CategoryService categoryService;

        /// <summary>
        /// Логирование ошибок
        /// </summary>
        private readonly ErrorLogger _errorLogger = new ErrorLogger();

        /// <summary>
        /// Конструктор создания контроллера
        /// </summary>
        /// <param name="context">Контекс для работы с БД</param>
        public CategoriesController(AppDbContext context)
        {
            categoryService = new CategoryService(context);
        }

        /// <summary>
        /// Получение списка категорий по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <returns>Категория с указанным идентификатором</returns>
        /// <response code="200">Найденная категория</response>
        /// <response code="204">Категория с указанным идентификатором не найдена</response>
        /// <response code="500">Ошибка при поиске категории</response> 
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            try
            {
                var result = await categoryService.GetCategoryById(id);
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result.ToCategoryDtoMap());
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

        /// <summary>
        /// Получение списка категорий по идентификатору пользователя
        /// </summary>
        /// <param name="id">Идентификтаор пользователя</param>
        /// <returns>Список категорий</returns>
        /// <response code="200">Найденный список категорий</response>
        /// <response code="204">Список категорий пуст</response>
        /// <response code="500">Ошибка при поиске категорий</response> 
        [HttpGet("ByUserId/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesByUserId(int id)
        {
            try
            {
                var categoryList = await categoryService.GetCategoriesByUserId(id);
                var categoryDtoList = categoryList.Select(c => c.ToCategoryDtoMap());
                if (categoryDtoList.Count() == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(categoryDtoList);
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

        /// <summary>
        /// Добавление категории
        /// </summary>
        /// <param name="categoryDto">Категория приходящая с веб</param>
        /// <returns>Добавленная категория</returns>
        /// <response code="200">Созданная категория</response>
        /// <response code="500">Ошибка при создании категории</response> 
        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CategoryDto>> PostCategory(CategoryDto categoryDto)
        {
            try
            {
                var category = await categoryService.AddCategory(categoryDto.ToCategoryMap());
                return Ok(category.ToCategoryDtoMap());
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    JsonSerializer.Serialize(categoryDto),
                    ex.Message);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Удаление категории по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <response code="200">Категория удалена</response> 
        /// <response code="500">Ошибка при удалении категории</response> 
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await categoryService.DeleteCategoryById(id);
                return Ok();
            } catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    "Id: " + JsonSerializer.Serialize(id),
                    ex.Message);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Изменение названия категории
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <param name="categoryDto">Класс категории приходящий с веб</param>
        /// <returns>Измененная категория</returns>
        /// <response code="200">Измененная категория</response> 
        /// <response code="500">Ошибка при удалении категории</response> 
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CategoryDto>> PutCategory(int id, CategoryDto categoryDto)
        {
            Category editedCategory;
            try
            {
                editedCategory = await categoryService.EditCategoryTitle(id, categoryDto.ToCategoryMap());
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name, 
                    "Id: " + id + ", " + JsonSerializer.Serialize(categoryDto), 
                    ex.Message);
                return Problem(ex.Message);
            }
            return Ok(editedCategory.ToCategoryDtoMap());
        }
    }
}
