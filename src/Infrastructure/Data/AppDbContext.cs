using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product configuration
            modelBuilder.Entity<Product>(entity =>
            {
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

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
                
                entity.HasIndex(e => e.Username)
                    .IsUnique();
                
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.HasIndex(e => e.Email)
                    .IsUnique();
                
                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);
                
                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("User");
                
                entity.Property(e => e.CreatedDate)
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.Property(e => e.UpdatedDate);
            });
        }
    }
}
