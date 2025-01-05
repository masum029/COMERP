namespace UI.Models
{
    public class About : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; } = true;
        public string ? Img { get; set; }
        public List<IFormFile>? FormFile { get; set; }

        // Foreign Key for Company
        public string? CompanyId { get; set; }
        public Company Company { get; set; }

        // Navigation Property for SubAbout
        public ICollection<SubAbout> SubAbouts { get; set; } = new List<SubAbout>();
    }

    public class SubAbout : BaseModel
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

    public class AboutTopic : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        // Foreign Key for SubAbout
        public string SubAboutId { get; set; }
        public SubAbout SubAbout { get; set; }
    }
}
