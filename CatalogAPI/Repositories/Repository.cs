using CatalogAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected CatalogAppDbContext _context;
        public Repository(CatalogAppDbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State= EntityState.Modified; // informando a alteracao da entidade
            _context.Set<T>().Update(entity);
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task<T> GetById(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
        }

        
    }
}
