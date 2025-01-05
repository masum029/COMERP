using COMERP.Entities.Base;
using System.Text.Json.Serialization;

namespace COMERP.Entities
{
    public class About : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; } = true;
        public string? Img { get; set; }

        // Foreign Key for Company
        public string? CompanyId { get; set; }
        public Company Company { get; set; }

        // Navigation Property for SubAbout
        public ICollection<SubAbout> SubAbouts { get; set; } = new List<SubAbout>();
    }

    public class SubAbout : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        // Foreign Key for About
        public string AboutId { get; set; }
        public About About { get; set; }

        // Navigation Property for AboutTopics
        public ICollection<AboutTopic> AboutTopics { get; set; } = new List<AboutTopic>();
    }

    public class AboutTopic : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        // Foreign Key for SubAbout
        public string SubAboutId { get; set; }
        public SubAbout SubAbout { get; set; }
    }


}
