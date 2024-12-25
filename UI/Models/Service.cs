namespace UI.Models
{
    public class Service : BaseModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? DurationHours { get; set; }
        public string? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
