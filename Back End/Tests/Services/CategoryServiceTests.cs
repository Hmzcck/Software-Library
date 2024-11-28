using AutoFixture;
using Back_End.Data.Repositories;
using Back_End.DTOs.Category;
using Back_End.Models;
using Back_End.Services.impl;
using FluentAssertions;
using Moq;
using Xunit;

namespace Back_End.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<ICacheService> _cacheMock;
        private readonly CategoryService _categoryService;
        private readonly Fixture _fixture;

        public CategoryServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _cacheMock = new Mock<ICacheService>();
            _categoryService = new CategoryService(_categoryRepositoryMock.Object, _cacheMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_WhenCacheExists_ShouldReturnCachedData()
        {
            // Arrange
            var cachedCategories = _fixture.CreateMany<CategoryResponseDto>(3).ToList();
            _cacheMock.Setup(x => x.GetAsync<List<CategoryResponseDto>>("categories"))
                .ReturnsAsync(cachedCategories);

            // Act
            var result = await _categoryService.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(cachedCategories);
            _categoryRepositoryMock.Verify(x => x.GetAllAsync(), Times.Never);
        }

        [Fact]
        public async Task GetAllAsync_WhenCacheDoesNotExist_ShouldFetchFromRepository()
        {
            // Arrange
            var categories = _fixture.CreateMany<CategoryModel>(3).ToList();
            _cacheMock.Setup(x => x.GetAsync<List<CategoryResponseDto>>("categories"))
                .ReturnsAsync((List<CategoryResponseDto>)null);
            _categoryRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllAsync();

            // Assert
            result.Should().HaveCount(3);
            _categoryRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
            _cacheMock.Verify(x => x.SetAsync("categories", It.IsAny<List<CategoryResponseDto>>(), It.IsAny<TimeSpan>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnCategory()
        {
            // Arrange
            var category = _fixture.Create<CategoryModel>();
            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(category);

            // Act
            var result = await _categoryService.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(category.Id);
            _categoryRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((CategoryModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _categoryService.GetByIdAsync(1));
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateAndInvalidateCache()
        {
            // Arrange
            var createDto = _fixture.Create<CreateCategoryRequestDto>();
            var createdCategory = _fixture.Create<CategoryModel>();
            _categoryRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<CategoryModel>(), It.IsAny<List<int>>()))
                .ReturnsAsync(createdCategory);

            // Act
            var result = await _categoryService.CreateAsync(createDto);

            // Assert
            result.Should().BeEquivalentTo(createdCategory);
            _cacheMock.Verify(x => x.ClearAsync("categories"), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithValidId_ShouldUpdateAndInvalidateCache()
        {
            // Arrange
            var existingCategory = _fixture.Create<CategoryModel>();
            var updateDto = _fixture.Create<UpdateCategoryRequestDto>();
            var updatedCategory = _fixture.Create<CategoryModel>();

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(existingCategory);
            _categoryRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CategoryModel>(), It.IsAny<UpdateCategoryRequestDto>()))
                .ReturnsAsync(updatedCategory);

            // Act
            var result = await _categoryService.UpdateAsync(1, updateDto);

            // Assert
            result.Should().BeEquivalentTo(updatedCategory);
            _cacheMock.Verify(x => x.ClearAsync("categories"), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteAndInvalidateCache()
        {
            // Arrange
            var category = _fixture.Create<CategoryModel>();
            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(category);
            _categoryRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<CategoryModel>()))
                .ReturnsAsync(category);

            // Act
            var result = await _categoryService.DeleteAsync(1);

            // Assert
            result.Should().BeEquivalentTo(category);
            _cacheMock.Verify(x => x.ClearAsync("categories"), Times.Once);
        }
    }
}