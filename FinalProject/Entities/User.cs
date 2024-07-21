using FinalProject.Model;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Entities
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }



        public DateTime DateTime { get; set; }

        public int TelNumber { get; set; }
        public List<BasketItem>? BasketItems { get; set; }

        public List<OrderItem>? OrderItems { get; set; }

        public List<Order>? Orders { get; set; }
    }
}
