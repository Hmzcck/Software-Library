using System.Security.Claims;
using AutoFixture;
using Back_End.Data.Repositories;
using Back_End.DTOs.Item;
using Back_End.Models;
using Back_End.Services.impl;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Back_End.Tests.Services
{
    public class UserFavoriteItemServiceTests
    {
        private readonly Mock<UserManager<UserModel>> _userManagerMock;
        private readonly Mock<IItemRepository> _itemRepositoryMock;
        private readonly Mock<IUserFavoriteItemRepository> _userFavoriteItemRepositoryMock;
        private readonly UserFavoriteItemService _userFavoriteItemService;
        private readonly IFixture _fixture;

        public UserFavoriteItemServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var userStoreMock = new Mock<IUserStore<UserModel>>();
            _userManagerMock = new Mock<UserManager<UserModel>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
            _itemRepositoryMock = new Mock<IItemRepository>();
            _userFavoriteItemRepositoryMock = new Mock<IUserFavoriteItemRepository>();

            _userFavoriteItemService = new UserFavoriteItemService(
                _userManagerMock.Object,
                _itemRepositoryMock.Object,
                _userFavoriteItemRepositoryMock.Object
            );
        }

        [Fact]
        public async Task GetUserFavoriteItems_ReturnsPaginatedItems_WhenUserExists()
        {
            // Arrange
            var user = _fixture.Create<UserModel>();
            var filter = _fixture.Create<ItemFilterDto>();
            var expectedResponse = _fixture.Create<PaginatedResponse<ItemResponseDto>>();
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", user.UserName)
            }));

            _userManagerMock.Setup(x => x.FindByNameAsync(user.UserName))
                .ReturnsAsync(user);
            _userFavoriteItemRepositoryMock.Setup(x => x.GetUserFavoriteItems(user.Id, filter))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _userFavoriteItemService.GetUserFavoriteItems(claims, filter);

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task AddUserFavoriteItem_AddsItem_WhenValidRequest()
        {
            // Arrange
            var user = _fixture.Create<UserModel>();
            var itemId = _fixture.Create<int>();
            var item = _fixture.Create<ItemModel>();
            var expectedFavorite = new UserFavoriteItem { UserId = user.Id, ItemId = itemId };
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", user.UserName)
            }));

            _userManagerMock.Setup(x => x.FindByNameAsync(user.UserName))
                .ReturnsAsync(user);
            _itemRepositoryMock.Setup(x => x.GetByIdAsync(itemId))
                .ReturnsAsync(item);
            _userFavoriteItemRepositoryMock.Setup(x => x.GetUserFavoriteItem(user.Id, itemId))
                .ReturnsAsync((UserFavoriteItem)null);
            _userFavoriteItemRepositoryMock.Setup(x => x.AddUserFavoriteItem(It.IsAny<UserFavoriteItem>()))
                .ReturnsAsync(expectedFavorite);

            // Act
            var result = await _userFavoriteItemService.AddUserFavoriteItem(claims, itemId);

            // Assert
            result.Should().BeEquivalentTo(expectedFavorite);
        }

        [Fact]
        public async Task AddUserFavoriteItem_ThrowsException_WhenItemNotFound()
        {
            // Arrange
            var user = _fixture.Create<UserModel>();
            var itemId = _fixture.Create<int>();
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", user.UserName)
            }));

            _userManagerMock.Setup(x => x.FindByNameAsync(user.UserName))
                .ReturnsAsync(user);
            _itemRepositoryMock.Setup(x => x.GetByIdAsync(itemId))
                .ReturnsAsync((ItemModel)null);

            // Act
            var action = async () => await _userFavoriteItemService.AddUserFavoriteItem(claims, itemId);

            // Assert
            await action.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("item was not found");
        }

        [Fact]
        public async Task RemoveUserFavoriteItem_RemovesItem_WhenExists()
        {
            // Arrange
            var user = _fixture.Create<UserModel>();
            var itemId = _fixture.Create<int>();
            var existingFavorite = new UserFavoriteItem { UserId = user.Id, ItemId = itemId };
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", user.UserName)
            }));

            _userManagerMock.Setup(x => x.FindByNameAsync(user.UserName))
                .ReturnsAsync(user);
            _userFavoriteItemRepositoryMock.Setup(x => x.GetUserFavoriteItem(user.Id, itemId))
                .ReturnsAsync(existingFavorite);
            _userFavoriteItemRepositoryMock.Setup(x => x.RemoveUserFavoriteItem(existingFavorite))
                .ReturnsAsync(existingFavorite);

            // Act
            var result = await _userFavoriteItemService.RemoveUserFavoriteItem(claims, itemId);

            // Assert
            result.Should().BeEquivalentTo(existingFavorite);
        }

        [Fact]
        public async Task RemoveUserFavoriteItem_ThrowsException_WhenNotFound()
        {
            // Arrange
            var user = _fixture.Create<UserModel>();
            var itemId = _fixture.Create<int>();
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", user.UserName)
            }));

            _userManagerMock.Setup(x => x.FindByNameAsync(user.UserName))
                .ReturnsAsync(user);
            _userFavoriteItemRepositoryMock.Setup(x => x.GetUserFavoriteItem(user.Id, itemId))
                .ReturnsAsync((UserFavoriteItem)null);

            // Act
            var action = async () => await _userFavoriteItemService.RemoveUserFavoriteItem(claims, itemId);

            // Assert
            await action.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("items was not found in favorites");
        }
    }
}