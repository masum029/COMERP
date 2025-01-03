namespace UI.Models
{
    public class Slider : BaseModel
    {
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? ImageUrl { get; set; }
        public string? LinkUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CompanyId { get; set; }
        public List<IFormFile>? FormFile { get; set; }
        public Company Company { get; set; }
    }
}
