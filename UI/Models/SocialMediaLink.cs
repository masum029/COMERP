namespace UI.Models
{
    public class SocialMediaLink : BaseModel
    {
        public string Platform { get; set; }
        public string LinkUrl { get; set; }
        public string IconUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; } = true;
        public string CompanyId { get; set; }
        public List<IFormFile>? FormFile { get; set; }
        public Company Company { get; set; }
    }
}
