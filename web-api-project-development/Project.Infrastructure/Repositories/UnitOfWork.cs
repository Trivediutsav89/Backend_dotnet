using Microsoft.EntityFrameworkCore.Storage;
using Project.Core.Interfaces.IRepositories;
using Project.Infrastructure.Data;
using Project.Infrastructure.Repositories;

namespace Project.Infrastructure.Repositories
{
    /// <summary>
    /// Unit of Work implementation for managing multiple repositories and database transactions
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        private IUserRepository? _userRepository;
        private IProductRepository? _productRepository;
        private ICategoryRepository? _categoryRepository;
        private IRoleRepository? _roleRepository;
        private IAuthRepository? _authRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUserRepository Users =>
            _userRepository ??= new UserRepository(_context);

        public IProductRepository Products =>
            _productRepository ??= new ProductRepository(_context);

        public ICategoryRepository Categories =>
            _categoryRepository ??= new CategoryRepository(_context);

        public IRoleRepository Roles =>
            _roleRepository ??= new RoleRepository(_context);

        public IAuthRepository Auth =>
            _authRepository ??= new AuthRepository(_context);

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                if (_transaction != null)
                {
                    await _transaction.CommitAsync(cancellationToken);
                }
            }
            catch
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync(cancellationToken);
                }
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync(cancellationToken);
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
            await _context.DisposeAsync();
        }
    }
}
