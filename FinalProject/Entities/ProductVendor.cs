namespace FinalProject.Entities
{
    public class ProductVendor : BaseEntity
    {
        public Vendor Vendor { get; set; }
        public Product Product { get; set; }
        public int VendorId { get; set; }
        public int ProductId { get; internal set; } //
    }
}
