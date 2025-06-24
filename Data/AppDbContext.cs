using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StyleSphere.Models.ProductEntity;
using StyleSphere.Models.UserEntity;
using StyleSphere.Models.CartEntity;


namespace StyleSphere.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<CartItem> CartItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {

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
