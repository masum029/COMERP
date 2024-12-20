using COMERP.Entities;

namespace COMERP.DTOs
{
    public class PageContentDto
    {
        public string? Id { get; set; }
        public string? PageName { get; set; }
        public string? SectionTitle { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; } = true;
        public string CompanyId { get; set; }
    }
}
