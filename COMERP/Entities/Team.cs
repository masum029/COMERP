using COMERP.Entities.Base;

namespace COMERP.Entities
{
    public class Team : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string LeadEmployeeId { get; set; }
        public Employee LeadEmployee { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
