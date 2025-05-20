using Microsoft.EntityFrameworkCore;
using MockPars.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MockPars.Infrastructure.Context;

namespace MockPars.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct)
        {
            return await _dbSet.ToListAsync(ct);
        }

        public virtual async Task<T> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _dbSet.FindAsync(id, ct);
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<bool> ExistsAsync(int id, CancellationToken ct)
        {
            return await _dbSet.FindAsync(id) != null;
        }

        public virtual async Task<T> AddAsync(T entity, CancellationToken ct)
        {
            await _dbSet.AddAsync(entity,ct);
    
            return entity;
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken ct)
        {
            _context.Update(entity);
           
        }

        public virtual async Task DeleteAsync(T entity, CancellationToken ct)
        {
            _dbSet.Remove(entity);
      
        }

        public IQueryable<T> GetAllWithIncludes(Func<IQueryable<T>, IQueryable<T>> includeFunc = null, Expression<Func<T, bool>> filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
