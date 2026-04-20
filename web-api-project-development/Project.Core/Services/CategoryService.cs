using Project.Core.Entities.Business;
using Project.Core.Entities.General;
using Project.Core.Interfaces.IMapper;
using Project.Core.Interfaces.IRepositories;
using Project.Core.Interfaces.IServices;

namespace Project.Core.Services
{
    public class CategoryService : BaseService<Category, CategoryViewModel>, ICategoryService
    {
        private readonly IBaseMapper<Category, CategoryViewModel> _categoryViewModelMapper;
        private readonly IBaseMapper<CategoryCreateViewModel, Category> _categoryCreateMapper;
        private readonly IBaseMapper<CategoryUpdateViewModel, Category> _categoryUpdateMapper;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IUserContext _userContext;

        public CategoryService(
            IBaseMapper<Category, CategoryViewModel> categoryViewModelMapper,
            IBaseMapper<CategoryCreateViewModel, Category> categoryCreateMapper,
            IBaseMapper<CategoryUpdateViewModel, Category> categoryUpdateMapper,
            IBaseRepository<Category> categoryRepository,
            IUserContext userContext)
            : base(categoryViewModelMapper, categoryRepository)
        {
            _categoryCreateMapper = categoryCreateMapper;
            _categoryUpdateMapper = categoryUpdateMapper;
            _categoryViewModelMapper = categoryViewModelMapper;
            _categoryRepository = categoryRepository;
            _userContext = userContext;
        }

        public async Task<CategoryViewModel> Create(CategoryCreateViewModel model, CancellationToken cancellationToken)
        {
            var entity = _categoryCreateMapper.MapModel(model);
            entity.EntryDate = DateTime.Now;
            entity.EntryBy = Convert.ToInt32(_userContext.UserId);

            return _categoryViewModelMapper.MapModel(await _categoryRepository.Create(entity, cancellationToken));
        }

        public async Task Update(CategoryUpdateViewModel model, CancellationToken cancellationToken)
        {
            var existingData = await _categoryRepository.GetById(model.Id, cancellationToken);
            _categoryUpdateMapper.MapModel(model, existingData);
            existingData.UpdatedDate = DateTime.Now;
            existingData.UpdatedBy = Convert.ToInt32(_userContext.UserId);

            await _categoryRepository.Update(existingData, cancellationToken);
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var existingData = await _categoryRepository.GetById(id, cancellationToken);
            await _categoryRepository.Delete(existingData, cancellationToken);
        }
    }
}
