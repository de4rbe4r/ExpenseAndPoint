﻿namespace ExpenseAndPointServer.Models
{
    public class Expense
    {
        public int Id { get; set; } 
        public float Amount { get; set; }
        public DateTime DateTime { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public Category? Category { get; set; }
        public User? User { get; set; }
    }
}
