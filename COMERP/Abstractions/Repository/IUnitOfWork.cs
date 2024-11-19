namespace COMERP.Abstractions.Repository
{
    public interface IUnitOfWork
    {
        ICompanyRepository CompanyRepository { get; }
        Task SaveAsync();
    }
}
