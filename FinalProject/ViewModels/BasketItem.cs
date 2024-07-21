using FinalProject.Entities;

namespace FinalProject.ViewModels
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public string UserId { get; set; }

        public int Count { get; set; }

        public Product Product { get; set; }

        public User User { get; set; }
    }
}
