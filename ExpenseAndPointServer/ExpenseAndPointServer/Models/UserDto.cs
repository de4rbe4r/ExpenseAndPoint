using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseAndPointServer.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public User ToUserMap()
        {
            return new User
            {
                Id = Id,
                Name = Name,
                Password = Password
            };
        }
    }
}
