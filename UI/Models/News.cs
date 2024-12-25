namespace UI.Models
{
    public class News : BaseModel
    {
        public string Title { get; set; }
        public string? Content { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
