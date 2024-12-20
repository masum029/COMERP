using COMERP.Entities;

namespace COMERP.DTOs
{
    public class CompanyDetailsDto
    {
        public string? Id { get; set; }
        public string CompanyId { get; set; }
        public string? Mission { get; set; }
        public string? Vision { get; set; }
        public string? CoreValues { get; set; }
    }
}
