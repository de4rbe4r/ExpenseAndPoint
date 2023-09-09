using ExpenseAndPoint.Data;
using ExpenseAndPointServer.Models;
using ExpenseAndPointServer.Services.Cryptographer;
using ExpenseAndPointServer.Services.PasswordChecker;
using Microsoft.AspNetCore.Mvc;

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

        public User AddUser(User user)
        {
            if (_context.Users == null) throw new Exception("Ссылка на 'AppDbContext.User' пустая");
            if (_context.Users.FirstOrDefault(u => u.Name == user.Name) != null) throw new Exception("Пользователь с таким именем уже существует");
            if (!_passwordChecker.IsStrengthPassword(user.Password)) throw new Exception("Пароль должен содержать буквы верхнего и нижнего регистра," +
                "хотя бы одну цифру и один специальный символ");
            user.Password = _cryptographer.Encrypt(user.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
    }
}
