using COMERP.Entities.Base;

namespace COMERP.Entities
{
    public class PageContent : BaseEntity
    {
        public string? PageName { get; set; }
        public string? SectionTitle { get; set; }
        public string? Content { get; set; }
        public string?   ImageUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; } = true;
        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
