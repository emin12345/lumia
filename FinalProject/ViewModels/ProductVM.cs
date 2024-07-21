using FinalProject.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 20)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string MiniDesc { get; set; }
        public bool InStock { get; set; }
        [NotMapped]
        public ICollection<int>? CategoriesIds { get; set; } = null!;
        [NotMapped]
        public ICollection<int>? BrandIds { get; set; } = null!;
        [NotMapped]
        public ICollection<int>? VendorIds { get; set; } = null!;
        [NotMapped]
        public IFormFile? MainPhoto { get; set; }
        [NotMapped]
        public IFormFile? HoverPhoto { get; set; }
        [NotMapped]
        public IFormFile? BasketPhoto { get; set; }
        [NotMapped]
        public ICollection<IFormFile>? Images { get; set; }
        [NotMapped]
        public ICollection<ProductImage>? AllImages { get; set; }
        [NotMapped]
        public List<int>? ImagesId { get; set; }
        [NotMapped]
        public string? ColorsSizesQuantity { get; set; }
        public string? ProductSizeColorDelete { get; set; }
        public ICollection<ProductSizeColor>? ProductSizeColors { get; set; }

        public ProductVM()
        {
            ProductSizeColors = new List<ProductSizeColor>();
        }
    }
}