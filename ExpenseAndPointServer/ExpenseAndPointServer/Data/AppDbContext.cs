using ExpenseAndPointServer.Models.Categories;
using ExpenseAndPointServer.Models.Users;
using ExpenseAndPointServer.Models.Expenses;

using Microsoft.EntityFrameworkCore;

namespace ExpenseAndPoint.Data
{
    /// <summary>
    /// Контекст работы с базой данных
    /// </summary>
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Расходы
        /// </summary>
        public DbSet<Expense> Expenses { get; set; }

        /// <summary>
        /// Категорияя
        /// </summary>
        public DbSet<Category> Categories { get; set; } 

        /// <summary>
        /// Пользователи
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// История расходов
        /// </summary>
        public DbSet<ExpenseHistory> ExpenseHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Expenses)
                .WithOne(e => e.User)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.User)
                .WithMany(u => u.Expenses)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Categories)
                .WithOne(e => e.User)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(u => u.ExpenseHistories)
                .WithOne(e => e.User)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ExpenseHistory>()
                .HasOne(e => e.User)
                .WithMany(u => u.ExpenseHistories)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
