using COMERP.Abstractions.Repository;
using COMERP.DataContext;
using COMERP.DTOs;
using COMERP.Entities;
using COMERP.Implementation.Repository.Base;

namespace COMERP.Implementation.Repository
{
    public class CompanyDetailsRepository : Repository<CompanyDetails>, ICompanyDetailsRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyDetailsRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }

        public Task<(bool Success, string id, string Message)> AddCompanyDetailsSqlAsync(CompanyDetailsDto model)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Success, string id, string Message)> UpdateCompanyDetailsSqlAsync(CompanyDetailsDto model)
        {
            throw new NotImplementedException();
        }
    }
}
