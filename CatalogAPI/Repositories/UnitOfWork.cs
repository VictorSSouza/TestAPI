using CatalogAPI.Data;

namespace CatalogAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ProductRepository? _prodRepos;
        private CategoryRepository? _categoryRepos;
        public CatalogAppDbContext _context;

        public UnitOfWork(CatalogAppDbContext context)
        {
            _context = context;
        }

        public IProductRepository ProductRepository
        {
            get {
                return _prodRepos = _prodRepos ?? new ProductRepository(_context);
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepos = _categoryRepos ?? new CategoryRepository(_context);
            }
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose() 
        {
             _context.Dispose();
        }
    }
}
