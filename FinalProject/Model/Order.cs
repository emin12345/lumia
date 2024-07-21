using FinalProject.Entities;

namespace FinalProject.Model
{
    public class Order : BaseEntity
    {


        public string Country { get; set; }

        public string Adress { get; set; }


        public string State { get; set; }


        public double TotalPrice { get; set; }

        public bool? Status { get; set; }
        public string? Message { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }

        public User User { get; set; }

        public List<OrderItem>? OrderItems { get; set; }

    }
}
