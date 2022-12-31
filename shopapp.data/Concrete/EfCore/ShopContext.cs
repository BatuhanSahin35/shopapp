using Microsoft.EntityFrameworkCore;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class ShopContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<ListItem> ListItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-87LVU45;Database=PlannerDB;Trusted_Connection=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                .HasKey(c => new { c.CategoryId, c.ProductId });
        }


    }
}