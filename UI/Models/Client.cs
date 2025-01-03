using System.ComponentModel;

namespace UI.Models
{
    public class Client : BaseModel
    {
        public string Name { get; set; }
        public string? ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public List<IFormFile>? FormFile { get; set; }
        public string? Icon { get; set; }
        public bool isActive { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
