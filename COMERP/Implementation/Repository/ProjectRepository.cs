using COMERP.Abstractions.Repository;
using COMERP.DataContext;
using COMERP.DTOs;
using COMERP.Entities;
using COMERP.Implementation.Repository.Base;

namespace COMERP.Implementation.Repository
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }

        public Task<(bool Success, string id, string Message)> AddProjectSqlAsync(ProjectDto model)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Success, string id, string Message)> UpdateProjectSqlAsync(ProjectDto model)
        {
            throw new NotImplementedException();
        }
    }
}
