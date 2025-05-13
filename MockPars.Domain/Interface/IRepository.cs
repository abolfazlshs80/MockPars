using System.Linq.Expressions;

namespace MockPars.Domain.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IQueryable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        public IQueryable<T> GetAllWithIncludes(
            Func<IQueryable<T>, IQueryable<T>> includeFunc = null,
            Expression<Func<T, bool>> filter = null);
    }
}
