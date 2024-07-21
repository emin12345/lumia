namespace FinalProject.Entities
{
    public class ProductCategory : BaseEntity
    {
        public Category Category { get; set; }
        public Product Product { get; set; }
        public int CategoryId { get; set; }
        public int ProductId { get; internal set; } //
    }
}
