using AutoFixture;
using Back_End.Data.Repositories;
using Back_End.DTOs.Item;
using Back_End.Models;
using Back_End.Services.impl;
using FluentAssertions;
using Moq;
using Xunit;

namespace Back_End.Tests.Services
{
    public class ItemServiceTests
    {
        private readonly Mock<IItemRepository> _itemRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<ICacheService> _cacheMock;
        private readonly ItemService _itemService;
        private readonly IFixture _fixture;

        public ItemServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _itemRepositoryMock = new Mock<IItemRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _cacheMock = new Mock<ICacheService>();
            _itemService = new ItemService(
                _itemRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _cacheMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsCachedItems_WhenCacheExists()
        {
            // Arrange
            var filter = _fixture.Create<ItemFilterDto>();
            var search = "test";
            var expectedResponse = _fixture.Create<PaginatedResponse<ItemResponseDto>>();
            
            _cacheMock.Setup(x => x.GetAsync<PaginatedResponse<ItemResponseDto>>(
                It.IsAny<string>())).ReturnsAsync(expectedResponse);

            // Act
            var result = await _itemService.GetAllAsync(filter, search);

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
            _itemRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<ItemFilterDto>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WhenItemNotFound()
        {
            // Arrange
            var id = _fixture.Create<int>();
            _itemRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((ItemModel)null);

            // Act
            var action = async () => await _itemService.GetByIdAsync(id);

            // Assert
            await action.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("item was not found");
        }

        [Fact]
        public async Task CreateAsync_CreatesItem_AndClearsCache()
        {
            // Arrange
            var createDto = _fixture.Create<CreateItemRequestDto>();
            var categories = _fixture.CreateMany<CategoryModel>().ToList();
            var createdItem = _fixture.Create<ItemModel>();

            _categoryRepositoryMock.Setup(x => x.GetCategoriesByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(categories);
            _itemRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<ItemModel>()))
                .ReturnsAsync(createdItem);

            // Act
            var result = await _itemService.CreateAsync(createDto);

            // Assert
            result.Should().BeEquivalentTo(createdItem);
            _cacheMock.Verify(x => x.ClearAsync("items_"), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesItem_WhenExists()
        {
            // Arrange
            var id = _fixture.Create<int>();
            var updateDto = _fixture.Create<UpdateItemRequestDto>();
            var existingItem = _fixture.Create<ItemModel>();
            var updatedItem = _fixture.Create<ItemModel>();

            _itemRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(existingItem);
            _itemRepositoryMock.Setup(x => x.UpdateAsync(existingItem, updateDto))
                .ReturnsAsync(updatedItem);

            // Act
            var result = await _itemService.UpdateAsync(id, updateDto);

            // Assert
            result.Should().BeEquivalentTo(updatedItem);
            _cacheMock.Verify(x => x.ClearAsync("items_"), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesItem_WhenExists()
        {
            // Arrange
            var id = _fixture.Create<int>();
            var existingItem = _fixture.Create<ItemModel>();

            _itemRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(existingItem);
            _itemRepositoryMock.Setup(x => x.DeleteAsync(existingItem))
                .ReturnsAsync(existingItem);

            // Act
            var result = await _itemService.DeleteAsync(id);

            // Assert
            result.Should().BeEquivalentTo(existingItem);
            _cacheMock.Verify(x => x.ClearAsync("items_"), Times.Once);
        }

        [Fact]
        public async Task AddCategoryAsync_AddsCategoryToItem_WhenBothExist()
        {
            // Arrange
            var request = _fixture.Create<AddCategoryRequestDto>();
            var existingItem = _fixture.Create<ItemModel>();
            var existingCategory = _fixture.Create<CategoryModel>();

            _itemRepositoryMock.Setup(x => x.GetByIdAsync(request.ItemId))
                .ReturnsAsync(existingItem);
            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(request.CategoryId))
                .ReturnsAsync(existingCategory);

            // Act
            var result = await _itemService.AddCategoryAsync(request);

            // Assert
            result.Should().BeEquivalentTo(existingItem);
            _itemRepositoryMock.Verify(x => x.AddCategoryAsync(existingItem, existingCategory), Times.Once);
            _cacheMock.Verify(x => x.ClearAsync("items_"), Times.Once);
        }
    }
}