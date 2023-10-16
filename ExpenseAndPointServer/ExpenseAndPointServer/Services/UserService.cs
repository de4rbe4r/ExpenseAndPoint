using ExpenseAndPoint.Data;
using ExpenseAndPointServer.Models;
using ExpenseAndPointServer.Services.Cryptographer;
using ExpenseAndPointServer.Services.PasswordChecker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ExpenseAndPointServer.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly ICryptographer _cryptographer;
        private readonly IPasswordChecker _passwordChecker;
        public UserService(AppDbContext context, ICryptographer cryptographer, IPasswordChecker passwordChecker)
        {
            _context = context;
            _cryptographer = cryptographer;
            _passwordChecker = passwordChecker;
        }

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

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User> GetUserByName(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
        }

        public async Task DeleteUserById(int id)
        {
            var user = await this.GetUserById(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> EditUserName(int id, User user)
        {
            if (id != user.Id) throw new Exception("Переданные Id и пользователь не совпадают! Проверьте отправляемые данные");
            if (GetUserByName(user.Name).Result != null) throw new Exception("Пользователь с таким именем уже существует");
            _context.Entry(user).Property(u => u.Name).IsModified = true;
            await _context.SaveChangesAsync();
            return await this.GetUserById(id);
        }

        public async Task<User> EditUserPassword(int id, User user)
        {
            if (id != user.Id) throw new Exception("Переданные Id и пользователь не совпадают! Проверьте отправляемые данные");
            _passwordChecker.CheckStrengthPassword(user.Password);
            user.Password = _cryptographer.Encrypt(user.Password);
            _context.Entry(user).Property(u => u.Password).IsModified = true;
            await _context.SaveChangesAsync();
            return await this.GetUserById(id);
        }
    }
}
