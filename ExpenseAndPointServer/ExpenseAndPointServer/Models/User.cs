using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseAndPointServer.Models
{
    public class User
    {
        public int Id { get; set; }
        [MinLength(4)]
        [NotNull]
        public string Name { get; set; }
        [MinLength(8)]
        [NotNull]
        public string Password { get; set; }
        public ICollection<Expense>? Expenses { get; set; }
    }
}
