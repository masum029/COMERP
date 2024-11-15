using COMERP.Entities.Base;

namespace COMERP.Entities
{
    public class Slider : BaseEntity
    {
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? ImageUrl { get; set; }
        public string? LinkUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
