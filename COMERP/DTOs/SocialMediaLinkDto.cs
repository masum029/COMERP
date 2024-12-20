namespace COMERP.DTOs
{
    public class SocialMediaLinkDto
    {
        public string? Id { get; set; }
        public string Platform { get; set; }
        public string LinkUrl { get; set; }
        public string IconUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; } = true;
        public string CompanyId { get; set; }
    }
}
