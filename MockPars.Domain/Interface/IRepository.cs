using System.Linq.Expressions;

namespace MockPars.Domain.Interface
{
    public interface IRepository<T> where T : class
    {
       
            Task<IEnumerable<T>> GetAllAsync();
            Task<T> GetByIdAsync(int id);
            Task<T> FindAsync(Expression<Func<T, bool>> predicate);
            Task<bool> ExistsAsync(int id);
            Task<T> AddAsync(T entity);
            Task UpdateAsync(T entity);
            Task DeleteAsync(T entity);
        
        public IQueryable<T> GetAllWithIncludes(
            Func<IQueryable<T>, IQueryable<T>> includeFunc = null,
            Expression<Func<T, bool>> filter = null);
    }
}
