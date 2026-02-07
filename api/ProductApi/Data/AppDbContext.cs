using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

namespace ProductApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Products tablosu
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product tablosu yapılandırması
            modelBuilder.Entity<Product>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.Id);
               
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
                
                entity.Property(e => e.Description)
                    .HasMaxLength(1000);
                
                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
                
                entity.Property(e => e.Stock)
                    .IsRequired();
               
                entity.Property(e => e.CreatedDate)
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
       
                entity.Property(e => e.UpdatedDate);
            });
        }
    }
}
