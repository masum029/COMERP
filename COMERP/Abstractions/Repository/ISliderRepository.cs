using COMERP.Abstractions.Repository.Base;
using COMERP.DTOs;
using COMERP.Entities;

namespace COMERP.Abstractions.Repository
{
    public interface ISliderRepository : IRepository<Slider>
    {
        // Add specific command methods here if needed
        Task<(bool Success, string id, string Message)> AddSliderSqlAsync(SliderDto model);
        Task<(bool Success, string id, string Message)> UpdateSliderSqlAsync(SliderDto model);
    }
}
