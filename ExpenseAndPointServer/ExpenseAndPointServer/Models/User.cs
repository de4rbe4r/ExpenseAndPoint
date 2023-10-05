using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseAndPointServer.Models
{
    public class User
    {
        public int Id { get; set; }
        [MinLength(4)]
        public string Name { get; set; }
        [MinLength(8)]
        public string Password { get; set; }
        public ICollection<Expense>? Expenses { get; set; }
        public ICollection<Category>? Categories { get; set; }
    }
}
