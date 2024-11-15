using COMERP.Entities.Base;

namespace COMERP.Entities
{
    public class Project : BaseEntity
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
