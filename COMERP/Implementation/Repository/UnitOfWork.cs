using COMERP.Abstractions.Repository;
using COMERP.DataContext;

namespace COMERP.Implementation.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ICompanyRepository companyRepository { get; private set; }

        public IClientRepository clientRepository { get; private set; }

        public ICompanyDetailsRepository companyDetailsRepository { get; private set; }

        public IContactFormSubmissionRepository contactFormSubmissionRepository { get; private set; }

        public IEventRepository eventRepository { get; private set; }

        public IFooterLinkRepository footerLinkRepository { get; private set; }

        public IMenuRepository menuRepository { get; private set; }

        public INewsRepository newsRepository { get; private set; }

        public IPageContentRepository pageContentRepository { get; private set; }

        public IProjectRepository projectRepository { get; private set; }

        public IServiceRepository serviceRepository { get; private set; }

        public ISiteSettingsRepository siteSettingsRepository { get; private set; }

        public ISliderRepository sliderRepository { get; private set; }

        public ISocialMediaLinkRepository socialMediaLinkRepository { get; private set; }

        public IAboutRepository aboutRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;
            companyRepository = new CompanyRepository(applicationDbContext,dapperDbContext, httpContextAccessor);
            clientRepository = new ClientRepository(applicationDbContext, dapperDbContext, httpContextAccessor);
            companyDetailsRepository = new CompanyDetailsRepository(applicationDbContext, dapperDbContext, httpContextAccessor);
            contactFormSubmissionRepository = new ContactFormSubmissionRepository(applicationDbContext, dapperDbContext, httpContextAccessor);
            eventRepository = new EventRepository(applicationDbContext, dapperDbContext, httpContextAccessor);
            footerLinkRepository = new FooterLinkRepository(applicationDbContext, dapperDbContext, httpContextAccessor);
            menuRepository = new MenuRepository(applicationDbContext, dapperDbContext, httpContextAccessor);
            newsRepository = new NewsRepository(applicationDbContext, dapperDbContext, httpContextAccessor);
            pageContentRepository = new PageContentRepository(applicationDbContext, dapperDbContext, httpContextAccessor);
            projectRepository = new ProjectRepository(applicationDbContext, dapperDbContext, httpContextAccessor);
            serviceRepository = new ServiceRepository(applicationDbContext, dapperDbContext, httpContextAccessor);
            siteSettingsRepository = new SiteSettingsRepository(applicationDbContext, dapperDbContext, httpContextAccessor);
            sliderRepository = new SliderRepository(applicationDbContext, dapperDbContext, httpContextAccessor);
            socialMediaLinkRepository = new SocialMediaLinkRepository(applicationDbContext, dapperDbContext, httpContextAccessor);
            aboutRepository = new AboutRepository(applicationDbContext, dapperDbContext, httpContextAccessor);


        }
        public async Task SaveAsync()
        {
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
