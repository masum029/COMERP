using COMERP.Entities.Base;

namespace COMERP.Entities
{
    public class TeamMember : BaseEntity
    {
        public string TeamId { get; set; }
        public Team Team { get; set; }
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Role { get; set; }
        public DateTime? JoinedDate { get; set; }
    }
}
 