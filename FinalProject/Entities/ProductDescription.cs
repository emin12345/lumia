namespace FinalProject.Entities
{
    public class ProductDescription : BaseEntity
    {
        public string Text { get; set; }
        public string ImagePath { get; set; }

        public Product Products { get; set; }
        public int ProductId { get; set; }
    }
}
