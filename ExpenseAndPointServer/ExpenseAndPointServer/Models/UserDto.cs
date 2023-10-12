using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseAndPointServer.Models
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
