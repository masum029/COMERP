using COMERP.Entities;

namespace COMERP.DTOs
{
    public class NewsDto
    {
        public string? Id { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string CompanyId { get; set; }
    }
}
