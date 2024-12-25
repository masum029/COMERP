namespace COMERP.DTOs
{
    public class FooterLinkDto
    {
        public string? Id { get; set; }
        public string Title { get; set; }
        public string? LinkUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; } 
        public string CompanyId { get; set; }
    }
}
