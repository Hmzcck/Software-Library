using AutoFixture;
using Back_End.Data;
using Back_End.Data.Repositories.impl;
using Back_End.DTOs.Category;
using Back_End.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Back_End.Tests.Repositories
{
    public class CategoryRepositoryTests : IDisposable
    {
        private readonly ApplicationDBContext _context;
        private readonly CategoryRepository _repository;
        private readonly Fixture _fixture;

        public CategoryRepositoryTests()
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
            _repository = new CategoryRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCategories()
        {
            // Arrange
            var categories = _fixture.CreateMany<CategoryModel>(3).ToList();
            await _context.Categories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(categories);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCategory_WhenExists()
        {
            // Arrange
            var category = _fixture.Create<CategoryModel>();
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(category.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(category);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddNewCategoryWithItems()
        {
            // Arrange
            var items = _fixture.CreateMany<ItemModel>(3).ToList();
            await _context.Items.AddRangeAsync(items);
            await _context.SaveChangesAsync();

            var category = _fixture.Create<CategoryModel>();
            var itemIds = items.Select(i => i.Id).ToList();

            // Act
            var result = await _repository.CreateAsync(category, itemIds);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(3);
            var savedCategory = await _context.Categories
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == result.Id);
            savedCategory.Should().NotBeNull();
            savedCategory!.Items.Should().BeEquivalentTo(items);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingCategory()
        {
            // Arrange
            var existingCategory = _fixture.Create<CategoryModel>();
            await _context.Categories.AddAsync(existingCategory);
            await _context.SaveChangesAsync();

            var updateDto = _fixture.Create<UpdateCategoryRequestDto>();

            // Act
            var result = await _repository.UpdateAsync(existingCategory, updateDto);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be(updateDto.Name);
            result.Description.Should().Be(updateDto.Description);

            var updatedCategory = await _context.Categories.FindAsync(existingCategory.Id);
            updatedCategory.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveCategory()
        {
            // Arrange
            var category = _fixture.Create<CategoryModel>();
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.DeleteAsync(category);

            // Assert
            result.Should().BeEquivalentTo(category);
            var deletedCategory = await _context.Categories.FindAsync(category.Id);
            deletedCategory.Should().BeNull();
        }

        [Fact]
        public async Task GetCategoriesByIdsAsync_ShouldReturnMatchingCategories()
        {
            // Arrange
            var categories = _fixture.CreateMany<CategoryModel>(5).ToList();
            await _context.Categories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            var selectedIds = categories.Take(3).Select(c => c.Id).ToList();

            // Act
            var result = await _repository.GetCategoriesByIdsAsync(selectedIds);

            // Assert
            result.Should().HaveCount(3);
            result.Select(c => c!.Id).Should().BeEquivalentTo(selectedIds);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}