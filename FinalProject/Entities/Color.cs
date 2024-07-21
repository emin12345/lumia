using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Entities
{
    public class Color : BaseEntity
    {
        public string Name { get; set; }
        public string ColorPath { get; set; }
        public ICollection<ProductSizeColor> ProductSizeColors { get; set; }
        [NotMapped]

        public IFormFile? Image { get; set; }
        public Color()
        {
            ProductSizeColors = new List<ProductSizeColor>();
        }
    }
}
