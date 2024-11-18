using System.Security.Claims;
using AutoFixture;
using Back_End.Controllers;
using Back_End.DTOs;
using Back_End.DTOs.Item;
using Back_End.Models;
using Back_End.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Back_End.Tests.Controllers
{
    public class UserFavoriteItemControllerTests
    {
        private readonly Mock<IUserFavoriteItemService> _mockUserFavoriteItemService;
        private readonly Mock<UserManager<UserModel>> _mockUserManager;
        private readonly UserFavoriteItemController _controller;
        private readonly Fixture _fixture;

        public UserFavoriteItemControllerTests()
        {
            _mockUserFavoriteItemService = new Mock<IUserFavoriteItemService>();
            
            // Setup mock UserManager
            var userStoreMock = new Mock<IUserStore<UserModel>>();
            _mockUserManager = new Mock<UserManager<UserModel>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _controller = new UserFavoriteItemController(
                _mockUserFavoriteItemService.Object,
                _mockUserManager.Object
            );
            
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        [Fact]
        public async Task GetUserFavoriteItems_ReturnsOkResult_WithPaginatedItems()
        {
            // Arrange
            var filterDto = _fixture.Create<ItemFilterDto>();
            var expectedResponse = _fixture.Create<PaginatedResponse<ItemResponseDto>>();
            _mockUserFavoriteItemService.Setup(s => s.GetUserFavoriteItems(It.IsAny<ClaimsPrincipal>(), filterDto))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetUserFavoriteItems(filterDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var items = okResult.Value.Should().BeAssignableTo<PaginatedResponse<ItemResponseDto>>().Subject;
            items.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task AddUserFavoriteItem_ReturnsCreatedResult_WhenSuccessful()
        {
            // Arrange
            var itemId = _fixture.Create<int>();
            var userFavoriteItem = _fixture.Create<UserFavoriteItem>();
            _mockUserFavoriteItemService.Setup(s => s.AddUserFavoriteItem(It.IsAny<ClaimsPrincipal>(), itemId))
                .ReturnsAsync(userFavoriteItem);

            // Act
            var result = await _controller.AddUserFavoriteItem(itemId);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.Value.Should().BeEquivalentTo(userFavoriteItem);
        }

        [Fact]
        public async Task RemoveUserFavoriteItem_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var itemId = _fixture.Create<int>();
            var userFavoriteItem = _fixture.Create<UserFavoriteItem>();
            _mockUserFavoriteItemService.Setup(s => s.RemoveUserFavoriteItem(It.IsAny<ClaimsPrincipal>(), itemId))
                .ReturnsAsync(userFavoriteItem);

            // Act
            var result = await _controller.RemoveUserFavoriteItem(itemId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task AddUserFavoriteItem_ReturnsNotFound_WhenItemNotFound()
        {
            // Arrange
            var itemId = _fixture.Create<int>();
            _mockUserFavoriteItemService.Setup(s => s.AddUserFavoriteItem(It.IsAny<ClaimsPrincipal>(), itemId))
                .ThrowsAsync(new KeyNotFoundException("item was not found"));

            // Act
            var result = async () => await _controller.AddUserFavoriteItem(itemId);

            // Assert
            await result.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("item was not found");
        }

        [Fact]
        public async Task AddUserFavoriteItem_ReturnsBadRequest_WhenItemAlreadyInFavorites()
        {
            // Arrange
            var itemId = _fixture.Create<int>();
            _mockUserFavoriteItemService.Setup(s => s.AddUserFavoriteItem(It.IsAny<ClaimsPrincipal>(), itemId))
                .ThrowsAsync(new ArgumentException("items is already in favorites"));

            // Act
            var result = async () => await _controller.AddUserFavoriteItem(itemId);

            // Assert
            await result.Should().ThrowAsync<ArgumentException>()
                .WithMessage("items is already in favorites");
        }

        [Fact]
        public async Task RemoveUserFavoriteItem_ReturnsNotFound_WhenItemNotFound()
        {
            // Arrange
            var itemId = _fixture.Create<int>();
            _mockUserFavoriteItemService.Setup(s => s.RemoveUserFavoriteItem(It.IsAny<ClaimsPrincipal>(), itemId))
                .ThrowsAsync(new KeyNotFoundException("items was not found in favorites"));

            // Act
            var result = async () => await _controller.RemoveUserFavoriteItem(itemId);

            // Assert
            await result.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("items was not found in favorites");
        }
    }
}