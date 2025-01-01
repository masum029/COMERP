using UI.Models;

namespace UI.ViewModel
{
    public class UiInfoVm
    {
        public Company Company { get; set; } = new Company();
        public IEnumerable<SocialMediaLink> SocialLink { get; set; }  =  new List<SocialMediaLink>();
    }
}
