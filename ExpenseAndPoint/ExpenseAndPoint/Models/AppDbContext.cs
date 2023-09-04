using Microsoft.EntityFrameworkCore;

namespace ExpenseAndPoint.Models
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; } 
    }
}
