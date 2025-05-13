namespace MockPars.Domain.Interface
{
    public interface IUnitOfWork : IDisposable
    {

    
        IRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
