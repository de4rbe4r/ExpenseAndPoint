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
            if (!_passwordChecker.IsStrengthPassword(user.Password)) throw new Exception("Пароль должен содержать буквы верхнего и нижнего регистра," +
                "хотя бы одну цифру и один специальный символ");
            if (user.Name.Length < 4) throw new Exception("Длина имени пользователя должна быть больше 4х символов");
            if (Regex.Match(user.Name, @".[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript).Success) throw new Exception("Имя пользователя не должен содержать " +
                "специальные символы");
            user.Password = _cryptographer.Encrypt(user.Password);
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
        public async Task<IEnumerable<User>> GetUserByName(string name)
        {
            return await _context.Users.Where(u => EF.Functions.Like(u.Name, $"%{name}%")).ToListAsync();
        }

        public async void DeleteUserById(int id)
        {
            var user = await this.GetUserById(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> EditUser(int id, User user)
        {
            if (id != user.Id) throw new Exception("Переданные Id и пользователь не совпадают! Проверьте отправляемые данные");

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return await this.GetUserById(id);
        }
    }
}
