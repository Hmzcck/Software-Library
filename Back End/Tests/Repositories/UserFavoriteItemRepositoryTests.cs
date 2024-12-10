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
    public class UserFavoriteItemRepositoryTests : IDisposable
    {
        private readonly ApplicationDBContext _context;
        private readonly UserFavoriteItemRepository _repository;
        private readonly Fixture _fixture;
        private readonly string _testUserId;

        public UserFavoriteItemRepositoryTests()
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
            _repository = new UserFavoriteItemRepository(_context);
            _testUserId = "test-user-id";
        }

        [Fact]
        public async Task GetUserFavoriteItems_ShouldReturnPaginatedItems()
        {
            // Arrange
            var items = _fixture.CreateMany<ItemModel>(10).ToList();
            await _context.Items.AddRangeAsync(items);

            var userFavoriteItems = items.Select(item => new UserFavoriteItem
            {
                UserId = _testUserId,
                ItemId = item.Id,
                Item = item
            }).ToList();

            await _context.UserFavoriteItems.AddRangeAsync(userFavoriteItems);
            await _context.SaveChangesAsync();

            var filterDto = new ItemFilterDto
            {
                PageNumber = 1,
                PageSize = 5
            };

            // Act
            var result = await _repository.GetUserFavoriteItems(_testUserId, filterDto);

            // Assert
            result.Items.Should().HaveCount(5);
            result.TotalCount.Should().Be(10);
            result.TotalPages.Should().Be(2);
            result.HasNext.Should().BeTrue();
            result.HasPrevious.Should().BeFalse();
        }

        [Fact]
        public async Task GetUserFavoriteItems_WithNameFilter_ShouldReturnFilteredItems()
        {
            // Arrange
            var items = _fixture.CreateMany<ItemModel>(5).ToList();
            items[0].Name = "TestName";
            await _context.Items.AddRangeAsync(items);

            var userFavoriteItems = items.Select(item => new UserFavoriteItem
            {
                UserId = _testUserId,
                ItemId = item.Id,
                Item = item
            }).ToList();

            await _context.UserFavoriteItems.AddRangeAsync(userFavoriteItems);
            await _context.SaveChangesAsync();

            var filterDto = new ItemFilterDto
            {
                Name = "TestName",
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = await _repository.GetUserFavoriteItems(_testUserId, filterDto);

            // Assert
            result.Items.Should().HaveCount(1);
            result.Items.First().Name.Should().Be("TestName");
        }

        [Fact]
        public async Task GetUserFavoriteItem_ShouldReturnItem_WhenExists()
        {
            // Arrange
            var item = _fixture.Create<ItemModel>();
            await _context.Items.AddAsync(item);

            var userFavoriteItem = new UserFavoriteItem
            {
                UserId = _testUserId,
                ItemId = item.Id,
                Item = item
            };

            await _context.UserFavoriteItems.AddAsync(userFavoriteItem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserFavoriteItem(_testUserId, item.Id);

            // Assert
            result.Should().NotBeNull();
            result!.ItemId.Should().Be(item.Id);
            result.UserId.Should().Be(_testUserId);
        }

        [Fact]
        public async Task GetUserFavoriteItem_ShouldReturnNull_WhenNotExists()
        {
            // Act
            var result = await _repository.GetUserFavoriteItem(_testUserId, 999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddUserFavoriteItem_ShouldAddNewFavorite()
        {
            // Arrange
            var userFavoriteItem = new UserFavoriteItem
            {
                UserId = _testUserId,
                ItemId = 1
            };

            // Act
            var result = await _repository.AddUserFavoriteItem(userFavoriteItem);

            // Assert
            result.Should().NotBeNull();
            var savedItem = await _context.UserFavoriteItems
                .FirstOrDefaultAsync(x => x.UserId == _testUserId && x.ItemId == 1);
            savedItem.Should().NotBeNull();
        }

        [Fact]
        public async Task RemoveUserFavoriteItem_ShouldRemoveFavorite()
        {
            // Arrange
            var userFavoriteItem = new UserFavoriteItem
            {
                UserId = _testUserId,
                ItemId = 1
            };
            await _context.UserFavoriteItems.AddAsync(userFavoriteItem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.RemoveUserFavoriteItem(userFavoriteItem);

            // Assert
            result.Should().NotBeNull();
            var removedItem = await _context.UserFavoriteItems
                .FirstOrDefaultAsync(x => x.UserId == _testUserId && x.ItemId == 1);
            removedItem.Should().BeNull();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}