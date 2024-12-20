using COMERP.Abstractions.Repository;
using COMERP.DataContext;
using COMERP.DTOs;
using COMERP.Entities;
using COMERP.Implementation.Repository.Base;

namespace COMERP.Implementation.Repository
{
    public class NewsRepository : Repository<News>, INewsRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NewsRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }

        public Task<(bool Success, string id, string Message)> AddNewsSqlAsync(NewsDto model)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Success, string id, string Message)> UpdateNewsSqlAsync(NewsDto model)
        {
            throw new NotImplementedException();
        }
    }
}
