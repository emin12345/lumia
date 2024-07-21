using FinalProject.Entities;
using FinalProject.Model;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.DAL
{
    public class LumiaDbContext : IdentityDbContext<User>
    {


        public LumiaDbContext(DbContextOptions<LumiaDbContext> options) : base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<AboutUs> AboutUs { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<BlogComment> BlogComments { get; set; }

        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductVendor> ProductVendors { get; set; }
        public DbSet<Category> Categories { get; set; }


        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Setting> Settings { get; set; }

        public DbSet<ContactUs> ContactUs { get; set; }


        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<ProductDescription> ProductDescriptions { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<WhislistItem> WhislistItemVMs { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ProductSizeColor> ProductSizeColors { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>().HasOne(o => o.User).WithMany(o => o.OrderItems).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.NoAction);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductDescriptions)
                .WithOne(pd => pd.Products)
                .HasForeignKey<Product>(p => p.ProductDescriptionsId);

            modelBuilder.Entity<ProductDescription>()
                .HasOne(pd => pd.Products)
                .WithOne(p => p.ProductDescriptions)
                .HasForeignKey<ProductDescription>(pd => pd.ProductId);

            modelBuilder.Entity<Setting>()
                .HasIndex(s => s.Key)
                .IsUnique();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Color>().HasIndex(t => t.Name).IsUnique();
            modelBuilder.Entity<Size>().HasIndex(c => c.Name).IsUnique();

        }





    }
}
