using COMERP.Entities.Base;

namespace COMERP.Entities
{
    public class ContactFormSubmission : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Message { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
