namespace FinalProject.Entities
{
    public class ProductBrand : BaseEntity
    {
        public Brand Brand { get; set; }
        public Product Product { get; set; }
        public int BrandId { get; set; }
        public int ProductId { get; internal set; } //
    }
}
