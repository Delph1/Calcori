using Calcori.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Calcori.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserMeal> UserMeals { get; set; }
        public DbSet<MealItem> MealItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MealItem>()
                .HasOne(m => m.UserMeal)
                .WithMany(m => m.MealItems)
                .HasForeignKey(m => m.UserMealId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
