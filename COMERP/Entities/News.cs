using COMERP.Entities.Base;

namespace COMERP.Entities
{
    public class News : BaseEntity
    {
        public string Title { get; set; }
        public string? Content { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
