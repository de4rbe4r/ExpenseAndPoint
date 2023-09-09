using ExpenseAndPoint.Data;
using ExpenseAndPointServer.Models;
using ExpenseAndPointServer.Services.Cryptographer;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseAndPointServer.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly ICryptographer _cryptographer;
        public UserService(AppDbContext context, ICryptographer cryptographer)
        {
            _context = context;
            _cryptographer = cryptographer;
        }

        public User AddUser(User user)
        {
            if (_context.User == null)
            {
                 return null;
            }
            user.Password = _cryptographer.Encrypt(user.Password);
            _context.User.Add(user);
            _context.SaveChanges();
            return user;
        }
    }
}
