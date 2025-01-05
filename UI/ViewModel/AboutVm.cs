using UI.Models;

namespace UI.ViewModel
{
    public class AboutVm
    {
        public About About { get; set; }
        public IEnumerable<SubAbout> subAbouts { get; set; }
        public IEnumerable<AboutTopic> aboutTopics { get; set; }
    }
}
