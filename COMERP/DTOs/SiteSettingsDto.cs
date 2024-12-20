using COMERP.Entities;

namespace COMERP.DTOs
{
    public class SiteSettingsDto
    {
        public string? Id { get; set; }
        public string CompanyId { get; set; }
        public string? LogoUrl { get; set; }
        public string? FaviconUrl { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
