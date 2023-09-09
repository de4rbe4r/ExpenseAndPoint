namespace ExpenseAndPointServer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public ICollection<Expense>? Expenses { get; set; }
    }
}
