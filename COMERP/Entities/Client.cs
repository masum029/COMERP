using COMERP.Entities.Base;

namespace COMERP.Entities
{
    public class Client : BaseEntity
    {
        public string Name { get; set; }
        public string? ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string? Icon { get; set; }
        public bool isActive { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
