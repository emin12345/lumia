using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class OrderVM
    {
        [System.ComponentModel.DataAnnotations.Required]
        [StringLength(maximumLength: 60)]
        public string Fullname { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [StringLength(maximumLength: 20)]
        public string Username { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [StringLength(maximumLength: 60)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [StringLength(maximumLength: 100)]
        public string Address { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [StringLength(maximumLength: 100)]
        public string Country { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [StringLength(maximumLength: 30)]
        public string State { get; set; }

        public List<BasketItem>? BasketItems { get; set; }

        //public List<WhislistItem>? WhislistItems { get; set; }


    }
}
