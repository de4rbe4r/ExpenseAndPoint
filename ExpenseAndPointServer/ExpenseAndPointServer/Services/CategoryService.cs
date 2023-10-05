using ExpenseAndPoint.Data;
using ExpenseAndPointServer.Models;
using ExpenseAndPointServer.Services.Cryptographer;
using ExpenseAndPointServer.Services.PasswordChecker;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ExpenseAndPointServer.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Category> AddCategory(Category category)
        {
            if (_context.Categories.FirstOrDefault(c => 
                                                        c.Title == category.Title 
                                                        && c.UserId == category.UserId) != null) 
               throw new Exception("Пользователь с таким именем уже существует");
            if (Regex.Match(category.Title, @".[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript).Success) 
                throw new Exception("Название категории не должно содержать специальные символы");
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> GetCategoriesByUserId(int userId)
        {
            return await _context.Categories.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _context.Categories.Include(c => c.Expenses).FirstOrDefaultAsync(u => u.Id == id) ?? throw new Exception("Категория с данным Id не существует");
        }

        public async Task<Category> EditCategory(int id, Category category)
        {
            if (id != category.Id) throw new Exception("Переданные Id и категория не совпадают! Проверьте отправляемые данные");

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return await this.GetCategoryById(id);
        }

        public async Task DeleteCategoryById(int id)
        {
            var category = await this.GetCategoryById(id);
            if (category.Expenses.Count > 0) throw new Exception("Нельзя удалить категорию, в которой есть расходы");
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }


    }
}
