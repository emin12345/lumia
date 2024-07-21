using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Entities
{
    public class AboutUs : BaseEntity
    {

        public string? ImagePath { get; set; }
        public string? Text { get; set; }
        public string? Author { get; set; }

        public string? IframeLink { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
