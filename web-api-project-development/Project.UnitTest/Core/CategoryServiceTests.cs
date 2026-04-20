using Project.Core.Entities.Business;
using Project.Core.Entities.General;
using Project.Core.Interfaces.IMapper;
using Project.Core.Interfaces.IRepositories;
using Project.Core.Interfaces.IServices;
using Project.Core.Services;
using Moq;

namespace Project.UnitTest
{
    public class CategoryServiceTests
    {
        private Mock<IBaseMapper<Category, CategoryViewModel>> _categoryViewModelMapperMock;
        private Mock<IBaseMapper<CategoryCreateViewModel, Category>> _categoryCreateMapperMock;
        private Mock<IBaseMapper<CategoryUpdateViewModel, Category>> _categoryUpdateMapperMock;
        private Mock<IBaseRepository<Category>> _categoryRepositoryMock;
        private Mock<IUserContext> _userContextMock;

        [SetUp]
        public void Setup()
        {
            _categoryViewModelMapperMock = new Mock<IBaseMapper<Category, CategoryViewModel>>();
            _categoryCreateMapperMock = new Mock<IBaseMapper<CategoryCreateViewModel, Category>>();
            _categoryUpdateMapperMock = new Mock<IBaseMapper<CategoryUpdateViewModel, Category>>();
            _categoryRepositoryMock = new Mock<IBaseRepository<Category>>();
            _userContextMock = new Mock<IUserContext>();
        }

        [Test]
        public async Task CreateCategoryAsync_ValidCategory_ReturnsCreatedCategoryViewModel()
        {
            // Arrange
            var categoryService = new CategoryService(
                _categoryViewModelMapperMock.Object,
                _categoryCreateMapperMock.Object,
                _categoryUpdateMapperMock.Object,
                _categoryRepositoryMock.Object,
                _userContextMock.Object);

            var newCategoryCreateViewModel = new CategoryCreateViewModel
            {
                Name = "Electronics",
                Description = "Electronic items",
                IsActive = true
            };

            var newCategory = new Category
            {
                Id = 1,
                Name = "Electronics",
                Description = "Electronic items",
                IsActive = true,
                EntryDate = DateTime.Now,
                EntryBy = 1
            };

            var newCategoryViewModel = new CategoryViewModel
            {
                Id = 1,
                Name = "Electronics",
                Description = "Electronic items",
                IsActive = true
            };

            _categoryCreateMapperMock.Setup(m => m.MapModel(newCategoryCreateViewModel)).Returns(newCategory);
            _userContextMock.Setup(u => u.UserId).Returns("1");
            _categoryRepositoryMock.Setup(r => r.Create(newCategory, It.IsAny<CancellationToken>())).ReturnsAsync(newCategory);
            _categoryViewModelMapperMock.Setup(m => m.MapModel(newCategory)).Returns(newCategoryViewModel);

            // Act
            var result = await categoryService.Create(newCategoryCreateViewModel, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Electronics", result.Name);
            Assert.AreEqual("Electronic items", result.Description);
        }

        [Test]
        public async Task UpdateCategoryAsync_ValidCategory_UpdatesSuccessfully()
        {
            // Arrange
            var categoryService = new CategoryService(
                _categoryViewModelMapperMock.Object,
                _categoryCreateMapperMock.Object,
                _categoryUpdateMapperMock.Object,
                _categoryRepositoryMock.Object,
                _userContextMock.Object);

            var updateCategoryViewModel = new CategoryUpdateViewModel
            {
                Id = 1,
                Name = "Updated Electronics",
                Description = "Updated description",
                IsActive = false
            };

            var existingCategory = new Category
            {
                Id = 1,
                Name = "Electronics",
                Description = "Electronic items",
                IsActive = true
            };

            _categoryRepositoryMock.Setup(r => r.GetById(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingCategory);
            _userContextMock.Setup(u => u.UserId).Returns("1");

            // Act
            await categoryService.Update(updateCategoryViewModel, CancellationToken.None);

            // Assert
            _categoryUpdateMapperMock.Verify(m => m.MapModel(updateCategoryViewModel, existingCategory), Times.Once);
            _categoryRepositoryMock.Verify(r => r.Update(existingCategory, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteCategoryAsync_ValidId_DeletesSuccessfully()
        {
            // Arrange
            var categoryService = new CategoryService(
                _categoryViewModelMapperMock.Object,
                _categoryCreateMapperMock.Object,
                _categoryUpdateMapperMock.Object,
                _categoryRepositoryMock.Object,
                _userContextMock.Object);

            // Act
            await categoryService.Delete(1, CancellationToken.None);

            // Assert
            _categoryRepositoryMock.Verify(r => r.Delete(1, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
