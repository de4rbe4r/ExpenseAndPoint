using ExpenseAndPoint.Data;
using ExpenseAndPointServer.Models.Categories;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ExpenseAndPointServer.Services
{

    /// <summary>
    /// Сервис работы с категориями
    /// </summary>
    public class CategoryService
    {
        /// <summary>
        /// Контекст для работы с БД
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Конструктор CategoryService
        /// </summary>
        /// <param name="context">Контекст для работы с БД</param>
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Добавление категории
        /// </summary>
        /// <param name="category">Модель категории для рбаоты с БД</param>
        /// <returns>Поток с созданной категорией Task-Category</returns>
        /// <exception cref="Exception">Ошибка наличия категории с таким же названием или ошибка в названии категории</exception>
        public async Task<Category> AddCategory(Category category)
        {
            if (_context.Users.FirstOrDefault(u => u.Id == category.UserId) == null) throw new Exception($"Пользователя с идентификатором {category.UserId} не существует");
            if (_context.Categories.FirstOrDefault(c => 
                                                        c.Title == category.Title 
                                                        && c.UserId == category.UserId) != null) 
               throw new Exception($"Категория с названием {category.Title} уже существует");
            if (Regex.Match(category.Title, @".[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript).Success) 
                throw new Exception("Название категории не должно содержать специальные символы");
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        /// <summary>
        /// Получение списка категорий по идентификатору пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Отдельный поток со списком категорий Task-Category</returns>
        /// <exception cref="Exception">Ошибка наличия пользователя с указанным идентификатором</exception>

        public async Task<IEnumerable<Category>> GetCategoriesByUserId(int userId)
        {
            if (_context.Users.FirstOrDefault(u => u.Id == userId) == null)
                throw new Exception($"Пользователя с идентификатором {userId} не существует");
            return await _context.Categories.Where(c => c.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Получение категории по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <returns></returns>
        /// <exception cref="Exception">Ошибка наличия категории с указанным идентификатором</exception>
        public async Task<Category> GetCategoryById(int id)
        {
            return await _context.Categories.Include(c => c.Expenses).FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <summary>
        /// Изменение названия категории
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <param name="category">Модель категории для работы с БД</param>
        /// <returns>Отдельный поток с измененной категорией</returns>
        /// <exception cref="Exception">Ошибка в переданных данных</exception>
        public async Task<Category> EditCategoryTitle(int id, Category category)
        {
            if (id != category.Id) throw new Exception("Переданные Id и категория не совпадают! Проверьте отправляемые данные");
            _context.Entry(category).Property(c => c.Title).IsModified = true;
            await _context.SaveChangesAsync();
            return await this.GetCategoryById(id);
        }

        /// <summary>
        /// Удаление категории по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <returns>Отдельный поток</returns>
        /// <exception cref="Exception">Ошибка удаления категории, содержащей расходы</exception>
        public async Task DeleteCategoryById(int id)
        {
            var category = await GetCategoryById(id);
            if (category.Expenses.Count > 0) throw new Exception("Нельзя удалить категорию, в которой есть расходы");
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
