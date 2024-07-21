using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Entities
{
    public class Blog
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedTime { get; set; }
        public string Description { get; set; }

        public string? ImagePath { get; set; }

        public string Text { get; set; }

        public bool Accept { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public List<BlogComment>? BlogComments { get; set; }



    }
}
