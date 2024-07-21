using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Entities
{
    public class Slider : BaseEntity
    {
        public int Id { get; set; }
        public string? ImagePath { get; set; }
        public string? Title { get; set; }
        public byte? Order { get; set; }
        public string? MiniDesc { get; set; }


        [NotMapped]
        public IFormFile? ImageFile { get; set; }

    }
}
