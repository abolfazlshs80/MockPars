using System.Linq.Expressions;

namespace MockPars.Domain.Interface
{
    public interface IRepository<T> where T : class
    {
       
            Task<IEnumerable<T>> GetAllAsync(CancellationToken ct);
            Task<T> GetByIdAsync(int id, CancellationToken ct);
            Task<T> FindAsync(Expression<Func<T, bool>> predicate);
            Task<bool> ExistsAsync(int id, CancellationToken ct);
            Task<T> AddAsync(T entity,CancellationToken ct);
            Task UpdateAsync(T entity, CancellationToken ct);
            Task DeleteAsync(T entity, CancellationToken ct);
        
        public IQueryable<T> GetAllWithIncludes(
            Func<IQueryable<T>, IQueryable<T>> includeFunc = null,
            Expression<Func<T, bool>> filter = null);
    }
}
