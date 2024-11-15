using COMERP.Entities.Base;

namespace COMERP.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string ? HeadEmployeeId { get; set; }
        public Employee HeadEmployee { get; set; }
    }
}
