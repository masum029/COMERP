namespace UI.Models
{
    public class Project : BaseModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Status { get; set; }
        public string? ClientId { get; set; }
        public Client? Client { get; set; }
        public string? CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
