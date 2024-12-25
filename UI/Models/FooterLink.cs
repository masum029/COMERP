namespace UI.Models
{
    public class FooterLink : BaseModel
    {
        public string Title { get; set; }
        public string? LinkUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; } 
        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
