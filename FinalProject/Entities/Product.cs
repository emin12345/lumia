using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MiniDesc { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        public bool InStock { get; set; }

        public string? BasketPhoto { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
        public List<ProductBrand> ProductBrands { get; set; }
        public List<ProductVendor> ProductVendors { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }

        public List<Comment> Comments { get; set; }
        public List<ProductSizeColor>? ProductSizeColors { get; set; }

        public ProductDescription ProductDescriptions { get; set; }


        public int ProductDescriptionsId { get; set; }





        public Product()
        {
            ProductImages = new List<ProductImage>();
            ProductVendors = new List<ProductVendor>();
            ProductBrands = new List<ProductBrand>();
            ProductCategories = new List<ProductCategory>();
            ProductSizeColors = new();

        }
    }
}

