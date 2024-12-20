namespace COMERP.DTOs
{
    public class EventDto
    {
        public string? Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? EventDate { get; set; }
        public string? Location { get; set; }
        public string CompanyId { get; set; }
    }
}
