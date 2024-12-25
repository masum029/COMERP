namespace UI.Models
{
    public class CompanyDetails : BaseModel
    {
        public string CompanyId { get; set; }
        public Company Company { get; set; }
        public string? Mission { get; set; }
        public string? Vision { get; set; }
        public string? CoreValues { get; set; }
    }
}
