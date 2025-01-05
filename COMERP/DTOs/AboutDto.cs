namespace COMERP.DTOs
{
    public class AboutDto
    {
        public string? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; } = true;
        public string? Img { get; set; }
        public string? CompanyId { get; set; }
        public ICollection<SubAboutDto> SubAbouts { get; set; } 
    }
    public class SubAboutDto
    {
        public string? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public ICollection<AboutTopicDto> AboutTopics { get; set; } = new List<AboutTopicDto>();
    }

    public class AboutTopicDto
    {
        public string? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
