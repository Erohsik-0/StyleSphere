using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StyleSphere.Models.ProductEntity;
using StyleSphere.Models.User;


namespace StyleSphere.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product", "productSchema");

                entity.HasKey(e => e.id);

                entity.Property(e => e.id).ValueGeneratedOnAdd();

                entity.Property(e => e.name).HasMaxLength(30).IsRequired();

                entity.Property(e => e.price).HasMaxLength(10).IsRequired();


                entity.Property(e => e.image).HasMaxLength(30).IsRequired();


                entity.Property(e => e.rating).HasMaxLength(5).IsRequired();


                entity.Property(e => e.description).HasMaxLength(1000).IsRequired();

                entity.Property(e => e.isAddedToCart).IsRequired();

            });
        }


    }
}
