namespace ExpenseAndPointServer.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Expense>? Expenses { get; set; }
        
    }
}
