using AutoFixture;
using Back_End.Data;
using Back_End.Data.Repositories.impl;
using Back_End.DTOs.Item;
using Back_End.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Back_End.Tests.Repositories
{
    public class ItemRepositoryTests : IDisposable
    {
        private readonly ApplicationDBContext _context;
        private readonly ItemRepository _repository;
        private readonly Fixture _fixture;

        public ItemRepositoryTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDBContext(options);
            _repository = new ItemRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPaginatedItems()
        {
            // Arrange
            var items = _fixture.CreateMany<ItemModel>(10).ToList();
            await _context.Items.AddRangeAsync(items);
            await _context.SaveChangesAsync();

            var filterDto = new ItemFilterDto
            {
                PageNumber = 1,
                PageSize = 5
            };

            // Act
            var result = await _repository.GetAllAsync(filterDto, "");

            // Assert
            result.Items.Should().HaveCount(5);
            result.TotalCount.Should().Be(10);
            result.TotalPages.Should().Be(2);
            result.HasNext.Should().BeTrue();
            result.HasPrevious.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllAsync_WithNameFilter_ShouldReturnFilteredItems()
        {
            // Arrange
            var items = _fixture.CreateMany<ItemModel>(5).ToList();
            items[0].Name = "TestName";
            await _context.Items.AddRangeAsync(items);
            await _context.SaveChangesAsync();

            var filterDto = new ItemFilterDto
            {
                Name = "TestName",
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = await _repository.GetAllAsync(filterDto, "");

            // Assert
            result.Items.Should().HaveCount(1);
            result.Items.First().Name.Should().Be("TestName");
        }

        [Fact]
        public async Task CreateAsync_ShouldAddNewItem()
        {
            // Arrange
            var item = _fixture.Create<ItemModel>();

            // Act
            var result = await _repository.CreateAsync(item);

            // Assert
            result.Should().NotBeNull();
            var savedItem = await _context.Items.FindAsync(result.Id);
            savedItem.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnItem_WhenExists()
        {
            // Arrange
            var item = _fixture.Create<ItemModel>();
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(item.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingItem()
        {
            // Arrange
            var existingItem = _fixture.Create<ItemModel>();
            await _context.Items.AddAsync(existingItem);
            await _context.SaveChangesAsync();

            var updateDto = _fixture.Create<UpdateItemRequestDto>();
            updateDto.CategoryIds = new List<int>(); 

            // Act
            var result = await _repository.UpdateAsync(existingItem, updateDto);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be(updateDto.Name);
            result.Description.Should().Be(updateDto.Description);
            result.Publisher.Should().Be(updateDto.Publisher);
            result.Stars.Should().Be(updateDto.Stars);
            result.Forks.Should().Be(updateDto.Forks);
            result.Repository.Should().Be(updateDto.Repository);
            result.Image.Should().Be(updateDto.Image);
            result.CreationDate.Should().Be(updateDto.CreationDate);

            // Verify persistence
            var updatedItemFromDb = await _context.Items.FindAsync(existingItem.Id);
            updatedItemFromDb.Should().NotBeNull();
            updatedItemFromDb.Should().BeEquivalentTo(result);

        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveItem()
        {
            // Arrange
            var item = _fixture.Create<ItemModel>();
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.DeleteAsync(item);

            // Assert
            result.Should().BeEquivalentTo(item);
            var deletedItem = await _context.Items.FindAsync(item.Id);
            deletedItem.Should().BeNull();
        }

        [Fact]
        public async Task AddCategoryAsync_ShouldAddCategoryToItem()
        {
            // Arrange
            var item = _fixture.Create<ItemModel>();
            var category = _fixture.Create<CategoryModel>();
            await _context.Items.AddAsync(item);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.AddCategoryAsync(item, category);

            // Assert
            result.Should().NotBeNull();
            result!.Categories.Should().Contain(category);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}