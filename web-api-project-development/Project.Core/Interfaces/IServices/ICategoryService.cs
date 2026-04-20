using Project.Core.Entities.Business;
using Project.Core.Entities.General;

namespace Project.Core.Interfaces.IServices
{
    public interface ICategoryService : IBaseService<CategoryViewModel>
    {
        Task<CategoryViewModel> Create(CategoryCreateViewModel model, CancellationToken cancellationToken);
        Task Update(CategoryUpdateViewModel model, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
    }
}
