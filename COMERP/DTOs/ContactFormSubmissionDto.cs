namespace COMERP.DTOs
{
    public class ContactFormSubmissionDto
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Message { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
        public string CompanyId { get; set; }
    }
}
