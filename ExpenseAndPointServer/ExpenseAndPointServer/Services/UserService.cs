using ExpenseAndPoint.Data;
using ExpenseAndPointServer.Models.Users;
using ExpenseAndPointServer.Models.Categories;
using ExpenseAndPointServer.Services.Cryptographer;
using ExpenseAndPointServer.Services.PasswordChecker;
using Microsoft.EntityFrameworkCore;

namespace ExpenseAndPointServer.Services
{

    /// <summary>
    /// Сервис работы с пользователями
    /// </summary>
    public class UserService
    {
        /// <summary>
        /// Контект для работы с БД
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Сервис шифрования пароля
        /// </summary>
        private readonly ICryptographer _cryptographer;

        /// <summary>
        /// Сервис проверки надежности пароля
        /// </summary>
        private readonly IPasswordChecker _passwordChecker;

        /// <summary>
        /// Конструктор UserService
        /// </summary>
        /// <param name="context">Контекст для работы с БД</param>
        /// <param name="cryptographer">Сервис шифрования пароля</param>
        /// <param name="passwordChecker">Сервис шифрования пароля</param>
        public UserService(AppDbContext context, ICryptographer cryptographer, IPasswordChecker passwordChecker)
        {
            _context = context;
            _cryptographer = cryptographer;
            _passwordChecker = passwordChecker;
        }

        /// <summary>
        /// Добавление пользователя
        /// </summary>
        /// <param name="user">Модель пользователя для работы с БД</param>
        /// <returns>Созданный пользователь</returns>
        /// <exception cref="Exception">Ошибки наличия пользователя с таким же именем или ошибка ненадежности пароля</exception>
        public async Task<User> AddUser(User user)
        {
            if (_context.Users.FirstOrDefault(u => u.Name == user.Name) != null) throw new Exception("Пользователь с таким именем уже существует");
            _passwordChecker.CheckStrengthPassword(user.Password);
            user.Password = _cryptographer.Encrypt(user.Password);
            user.Categories = new List<Category>
            {
                new Category { Title = "Продукты" },
                new Category { Title = "Транспорт" },
                new Category { Title = "Аптека" }
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <returns>Коллекция пользователей</returns>
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Получение пользователя по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns>Пользователь</returns>
        /// <exception cref="Exception">Ошибка наличия пользователя с указанным идентификатором</exception>

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id) ?? throw new Exception("Пользователя с данным Id не существует");
        }

        /// <summary>
        /// Получение пользователя по имени
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        /// <returns>Пользователь</returns>
        /// <exception cref="Exception">Ошибка наличия пользователя с указанным именем</exception>
        public async Task<User> GetUserByName(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
        }

        /// <summary>
        /// Удаление пользователя по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        public async Task DeleteUserById(int id)
        {
            var user = await this.GetUserById(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Изменение имени пользователя
        /// </summary>
        /// <param name="id">Идентификтор пользователя</param>
        /// <param name="user">Модель пользователя для работы с БД</param>
        /// <returns>Измененный пользователь</returns>
        /// <exception cref="Exception">Ошибки в переданных данных или ошибка наличия пользователя с таким же именем</exception>
        public async Task<User> EditUserName(int id, User user)
        {
            if (id != user.Id) throw new Exception("Переданные Id и пользователь не совпадают! Проверьте отправляемые данные");
            if (GetUserByName(user.Name).Result != null) throw new Exception("Пользователь с таким именем уже существует");
            _context.Entry(user).Property(u => u.Name).IsModified = true;
            await _context.SaveChangesAsync();
            return await GetUserById(id);
        }

        /// <summary>
        /// Изменение пароля
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="user">Модель пользователя для работы с БД</param>
        /// <returns>Измененный пользователь</returns>
        /// <exception cref="Exception">Ошибки наличия пользователя с таким же именем и ненадежности пароля</exception>
        public async Task<User> EditUserPassword(int id, User user)
        {
            if (id != user.Id) throw new Exception("Переданные Id и пользователь не совпадают! Проверьте отправляемые данные");
            _passwordChecker.CheckStrengthPassword(user.Password);
            user.Password = _cryptographer.Encrypt(user.Password);
            _context.Entry(user).Property(u => u.Password).IsModified = true;
            await _context.SaveChangesAsync();
            return await GetUserById(id);
        }
    }
}
