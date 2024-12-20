namespace COMERP.DTOs
{
    public class ServiceDto
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? DurationHours { get; set; }
        public string? CompanyId { get; set; }
    }
}
