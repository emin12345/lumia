using System.ComponentModel.DataAnnotations;

namespace FinalProject.Entities
{
    public class BlogComment
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string Text { get; set; }
        public DateTime CreatedTime { get; set; }

        public bool IsAccess { get; set; }
        [Required]
        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
