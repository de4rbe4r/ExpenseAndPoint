namespace ExpenseAndPoint.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Expense>? Expenses { get; set; }
    }
}
