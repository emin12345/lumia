using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class RegisterVM
    {
        public string Firstname { get; set; }
        [StringLength(maximumLength: 15)]
        public string Username { get; set; }

        public string Lastname { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }


        public string PhoneNumber { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
