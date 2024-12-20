namespace COMERP.Abstractions.Repository
{
    public interface IUnitOfWork
    {
        ICompanyRepository companyRepository { get; }
        IClientRepository clientRepository { get; }
        ICompanyDetailsRepository companyDetailsRepository { get; }
        IContactFormSubmissionRepository contactFormSubmissionRepository { get; }
        IEventRepository eventRepository { get; }
        IFooterLinkRepository footerLinkRepository { get; }
        IMenuRepository menuRepository { get; }
        INewsRepository newsRepository { get; }
        IPageContentRepository pageContentRepository { get; }
        IProjectRepository projectRepository { get; }
        IServiceRepository serviceRepository { get; }
        ISiteSettingsRepository siteSettingsRepository { get; }
        ISliderRepository sliderRepository { get; }
        ISocialMediaLinkRepository socialMediaLinkRepository { get; }
        Task SaveAsync();
    }
}
