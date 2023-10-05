using ExpenseAndPoint.Data;
using ExpenseAndPointServer.Models;
using ExpenseAndPointServer.Services;
using ExpenseAndPointServer.Services.Cryptographer;
using ExpenseAndPointServer.Services.PasswordChecker;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseAndPointServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private CategoryService categoryService;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
            categoryService = new CategoryService(context);
        }

        // GET: api/Categories/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetCategoriesById(int id)
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
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // GET: api/Categories/ByUserId/5
        [HttpGet("ByUserId/{id:int}")]
        public async Task<ActionResult<User>> GetCategoriesByUserId(int id)
        {
            try
            {
                var result = await categoryService.GetCategoriesByUserId(id);
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            try
            {
                await categoryService.AddCategory(category);
                return CreatedAtAction(nameof(GetCategoriesById), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await categoryService.DeleteCategoryById(id);
            return NoContent();
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> PutCategory(int id, Category category)
        {
            Category editedCategory;
            try
            {
                editedCategory = await categoryService.EditCategory(id, category);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return Ok(editedCategory);
        }
    }
}
