using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;
using UI.ViewModel;

namespace UI.Services.Implementations
{
    public class SessionServices : ISessionServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApiUrlSettings _apiUrls;
        private readonly IClientServices<Company> _clientServices;
        private readonly IClientServices<SocialMediaLink> _socilServices;
        private readonly IClientServices<Slider> _sliderServices;
        private readonly IClientServices<Client> _clientsServices;
        private readonly IClientServices<Service> _serviceServices;
        private readonly IClientServices<FooterLink> _footerLinkServices;
        private readonly IClientServices<Menu> _menuServices;
        private readonly IClientServices<About> _AboutServices;

        private const string CompanyInfoKey = "CompanyInfo";
        private const string OldCompanyNameKey = "OldCompanyName";

        public SessionServices(IHttpContextAccessor httpContextAccessor,
            IOptions<ApiUrlSettings> apiUrls,
            IClientServices<Company> clientServices,
            IClientServices<SocialMediaLink> clintServices,
            IClientServices<Slider> sliderServices,
            IClientServices<Client> clientsServices,
            IClientServices<Service> serviceServices,
            IClientServices<FooterLink> footerLinkServices,
            IClientServices<Menu> menuServices,
            IClientServices<About> aboutServices)
        {
            _httpContextAccessor = httpContextAccessor;
            _apiUrls = apiUrls.Value;
            _clientServices = clientServices;
            _socilServices = clintServices;
            _sliderServices = sliderServices;
            _clientsServices = clientsServices;
            _serviceServices = serviceServices;
            _footerLinkServices = footerLinkServices;
            _menuServices = menuServices;
            _AboutServices = aboutServices;
        }

        /// <summary>
        /// Deletes session data related to the company.
        /// </summary>
        /// <returns>True if session data is removed successfully, otherwise false.</returns>
        public bool DeleteSessonData()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.Remove(CompanyInfoKey);
                session.Remove(OldCompanyNameKey);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retrieves the company object from the session. Updates session if data is missing or outdated.
        /// </summary>
        /// <returns>Company object from session or null if unavailable.</returns>
        public async Task<UiInfoVm?> GetCompanyFromSession()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return null;

            var companyJson = session.GetString(CompanyInfoKey);
            var oldCompanyName = session.GetString(OldCompanyNameKey);

            // Check if session data is missing or outdated
            if (string.IsNullOrEmpty(companyJson) ||
                JsonConvert.DeserializeObject<UiInfoVm>(companyJson)?.Company.Name != oldCompanyName)
            {
                var isUpdated = await AddCompanyToSessionAsync();
                if (!isUpdated) return null;

                // Re-fetch the updated data from the session
                companyJson = session.GetString(CompanyInfoKey);
            }
            return string.IsNullOrEmpty(companyJson)
                ? null
                : JsonConvert.DeserializeObject<UiInfoVm>(companyJson);
        }


        /// <summary>
        /// Fetches the active company data and updates the session.
        /// </summary>
        /// <returns>True if session data is updated successfully, otherwise false.</returns>
        public async Task<bool> AddCompanyToSessionAsync()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return false;

            var companyResponse = await _clientServices.GetAllClientsAsync(_apiUrls._CompanyUrl);
            var activeCompany = companyResponse?.Data?.FirstOrDefault(c => c.isActive);

            var SocialMedia = await _socilServices.GetAllClientsAsync(_apiUrls._SocialMediaLinkUrl);
            var filterSocialMedia = SocialMedia?.Data?
                .Where(s=>s.IsVisible && s.CompanyId == activeCompany?.Id)
                .OrderBy(s=>s.DisplayOrder)
                .ToList();

            var Slayders = await _sliderServices.GetAllClientsAsync(_apiUrls._SliderUrl);
            var slayderList = Slayders?.Data?
                .Where(sli => sli.IsActive && sli.CompanyId == activeCompany?.Id)
                .OrderBy(sli => sli.DisplayOrder)
                .ToList();

            var Clients = await _clientsServices.GetAllClientsAsync(_apiUrls._ClientUrl);
            var ClientsList = Clients?.Data?
                .Where(cli => cli.isActive && cli.CompanyId == activeCompany?.Id)
                .ToList();
            var Services = await _serviceServices.GetAllClientsAsync(_apiUrls._ServiceUrl);
            var ServicesList = Services?.Data?
                .Where(srv => srv.CompanyId == activeCompany?.Id)
                .ToList();

            var FooterLinks = await _footerLinkServices.GetAllClientsAsync(_apiUrls._FooterLinkUrl);
            var FooterLinksList = FooterLinks?.Data?
                 .Where(fl => fl.IsVisible && fl.CompanyId == activeCompany?.Id)
                .OrderBy(fl => fl.DisplayOrder)
                .ToList();
            var Menus = await _menuServices.GetAllClientsAsync(_apiUrls._MenuUrl);
            var MenusList = Menus?.Data?
                 .Where(m => m.IsVisible && m.CompanyId == activeCompany?.Id)
                .OrderBy(m => m.DisplayOrder)
                .ToList();

            var about = await _AboutServices.GetAllClientsAsync(_apiUrls._AboutUrl);
            var aboutList = about?.Data?
                 .FirstOrDefault(ab => ab.IsVisible);

            if (activeCompany != null)
            {
                var uiInfoVm = new UiInfoVm
                {
                    Company = activeCompany,
                    SocialLink = filterSocialMedia,
                    Sliders= slayderList,
                    Clients= ClientsList,
                    Services= ServicesList,
                    FooterLinks= FooterLinksList,
                    Menus= MenusList,
                    Abouts = aboutList
                };

                session.SetString(CompanyInfoKey, JsonConvert.SerializeObject(uiInfoVm));
                session.SetString(OldCompanyNameKey, activeCompany.Name);

                return true;
            }

            return false;
        }
    }
}