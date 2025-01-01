using UI.Models;
using UI.ViewModel;

namespace UI.Services.Interface
{
    public interface ISessionServices
    {
        Task<UiInfoVm> GetCompanyFromSession();
        bool DeleteSessonData();
        Task<bool> AddCompanyToSessionAsync();
    }
}
