using System.ComponentModel;

namespace UI.Models
{
    public class Client : BaseModel
    {
        public string Name { get; set; }
        [DisplayName("Contact Person")]
        public string? ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        [DisplayName("Company")]
        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
