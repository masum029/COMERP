using COMERP.Entities.Base;

namespace COMERP.Entities
{
    public class Employee : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string? Department { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? HireDate { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
