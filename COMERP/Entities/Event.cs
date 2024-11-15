using COMERP.Entities.Base;

namespace COMERP.Entities
{
    public class Event : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? EventDate { get; set; }
        public string? Location { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
