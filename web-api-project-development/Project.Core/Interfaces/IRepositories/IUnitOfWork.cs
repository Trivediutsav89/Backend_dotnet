namespace Project.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Unit of Work pattern interface for managing multiple repositories and transactions
    /// </summary>
    public interface IUnitOfWork : IAsyncDisposable
    {
        IUserRepository Users { get; }
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IRoleRepository Roles { get; }
        IAuthRepository Auth { get; }

        /// <summary>
        /// Begins a new transaction
        /// </summary>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        Task CommitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        Task RollbackAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves all changes to the database
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
