using System.ComponentModel.DataAnnotations;

namespace FinalProject.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string Text { get; set; }
        public DateTime CreatedTime { get; set; }

        public bool IsAccess { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
