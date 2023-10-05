using ExpenseAndPointServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseAndPoint.Data
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; } 
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.User)
                .WithMany(u => u.Expenses)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Category>()
                .HasOne(e => e.User)
                .WithMany(u => u.Categories)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
