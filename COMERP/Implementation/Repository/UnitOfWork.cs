using COMERP.Abstractions.Repository;
using COMERP.DataContext;

namespace COMERP.Implementation.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ICompanyRepository CompanyRepository { get; private set; }
        public UnitOfWork(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;
            CompanyRepository = new CompanyRepository(applicationDbContext,dapperDbContext, httpContextAccessor);
           
        }
        public async Task SaveAsync()
        {
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
