using COMERP.Abstractions.Repository;
using COMERP.DataContext;
using COMERP.DTOs;
using COMERP.Entities;
using COMERP.Exceptions;
using COMERP.Implementation.Repository.Base;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class AboutRepository : Repository<About>, IAboutRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly DapperDbContext _dapperDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AboutRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
        : base(applicationDbContext, dapperDbContext, httpContextAccessor)
    {
        _applicationDbContext = applicationDbContext;
        _dapperDbContext = dapperDbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<(bool Success, string id, string Message)> AddAboutSqlAsync(AboutDto model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        var about = new About
        {
            Title = model.Title,
            Description = model.Description,
            IsVisible = model.IsVisible,
            CompanyId = model.CompanyId,
            Img = model.Img,
           
            SubAbouts = model.SubAbouts.Select(sa => new SubAbout
            {
                Title = sa.Title,
                Description = sa.Description,
                Icon = sa.Icon,
                AboutTopics = sa.AboutTopics.Select(at => new AboutTopic
                {
                    Title = at.Title,
                    Description = at.Description,
                    Icon = at.Icon
                }).ToList()
            }).ToList()
        };
        about.SetCreatedDate(DateTime.UtcNow, GetUserName());
        await _applicationDbContext.Abouts.AddAsync(about);
        var result = await _applicationDbContext.SaveChangesAsync();

        if (result > 0)
        {
            return (true, about.Id, "About section added successfully.");
        }
        return (false, null, "Failed to add About section.");
    }

    public async Task<About> GetAboutByIdSqlAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            throw new ArgumentNullException(nameof(id));

        var about = await _applicationDbContext.Abouts
            .Where(a => a.Id == id)
            .Select(a => new About
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                IsVisible = a.IsVisible,
                CompanyId=a.CompanyId,
                Img=a.Img,
                SubAbouts = a.SubAbouts.Select(sa => new SubAbout
                {
                    Id = sa.Id,
                    Title = sa.Title,
                    Description = sa.Description,
                    Icon = sa.Icon,
                    AboutTopics = sa.AboutTopics.Select(at => new AboutTopic
                    {
                        Id = at.Id,
                        Title = at.Title,
                        Description = at.Description,
                        Icon = at.Icon
                    }).ToList()
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (about == null)
            throw new NotFoundException($"About with ID {id} not found.");

        return about;
    }

    public async Task<IEnumerable<About>> GetAboutSqlAsync()
    {
        var abouts = await _applicationDbContext.Abouts
            .Select(a => new About
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                IsVisible = a.IsVisible,
                Img = a.Img,
                SubAbouts = a.SubAbouts.Select(sa => new SubAbout
                {
                    Id = sa.Id,
                    Title = sa.Title,
                    Description = sa.Description,
                    Icon = sa.Icon,
                    AboutTopics = sa.AboutTopics.Select(at => new AboutTopic
                    {
                        Id = at.Id,
                        Title = at.Title,
                        Description = at.Description,
                        Icon = at.Icon
                    }).ToList()
                }).ToList()
            }).ToListAsync();

        return abouts;
    }


    public async Task<(bool Success, string id, string Message)> UpdateAboutSqlAsync(AboutDto model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        var about = await _applicationDbContext.Abouts
            .Include(a => a.SubAbouts)
                .ThenInclude(sa => sa.AboutTopics)
            .FirstOrDefaultAsync(a => a.Id == model.Id);

        if (about == null) throw new NotFoundException($"About with ID {model.Id} not found.");

        about.Title = model.Title;
        about.Description = model.Description;
        about.IsVisible = model.IsVisible;
        about.Img = model.Img;
        about.SetUpdateDate(DateTime.UtcNow, GetUserName());
        // Update SubAbouts and AboutTopics
        _applicationDbContext.SubAbouts.RemoveRange(about.SubAbouts);

        about.SubAbouts = model.SubAbouts.Select(sa => new SubAbout
        {
            Title = sa.Title,
            Description = sa.Description,
            Icon = sa.Icon,
            AboutTopics = sa.AboutTopics.Select(at => new AboutTopic
            {
                Title = at.Title,
                Description = at.Description,
                Icon = at.Icon
            }).ToList()
        }).ToList();

        var result = await _applicationDbContext.SaveChangesAsync();

        if (result > 0)
        {
            return (true, about.Id, "About section updated successfully.");
        }
        return (false, null, "Failed to update About section.");
    }

    private string GetUserName() =>
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";

    

}

