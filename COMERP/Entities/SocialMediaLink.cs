using COMERP.Entities.Base;

namespace COMERP.Entities
{
    public class SocialMediaLink : BaseEntity
    {
        public string Platform { get; set; }
        public string LinkUrl { get; set; }
        public string IconUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; } = true;
        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
