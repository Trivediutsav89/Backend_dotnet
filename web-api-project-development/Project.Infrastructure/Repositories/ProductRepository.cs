using Project.Core.Entities.General;
using Project.Core.Exceptions;
using Project.Core.Interfaces.IRepositories;
using Project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Project.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<double> PriceCheck(int productId, CancellationToken cancellationToken = default)
        {
            var product = await _dbContext.Products
                .Where(x => x.Id == productId)
                .Select(x => new { x.Price })
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                throw new NotFoundException("No data found");
            }

            return product.Price;
        }

    }
}
