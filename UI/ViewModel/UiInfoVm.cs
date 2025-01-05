using UI.Models;

namespace UI.ViewModel
{
    public class UiInfoVm
    {
        public Company Company { get; set; } = new Company();
        public IEnumerable<SocialMediaLink> SocialLink { get; set; }  =  new List<SocialMediaLink>();
        public IEnumerable<Slider> Sliders { get; set; }  =  new List<Slider>();
        public IEnumerable<Client> Clients { get; set; }  =  new List<Client>();
        public IEnumerable<Service> Services { get; set; }  =  new List<Service>();
        public IEnumerable<FooterLink> FooterLinks { get; set; }  =  new List<FooterLink>();
        public IEnumerable<Menu> Menus { get; set; }  =  new List<Menu>();
        public About Abouts { get; set; }  =  new About();
    }
}
