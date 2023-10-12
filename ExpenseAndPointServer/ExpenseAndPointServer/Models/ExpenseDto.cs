namespace ExpenseAndPointServer.Models
{
    public class ExpenseDto
    {
        public float Amount { get; set; }
        public DateTime DateTime { get; set; }
        public string CategoryTitle { get; set; }
        public string UserName { get; set; }
    }
}
