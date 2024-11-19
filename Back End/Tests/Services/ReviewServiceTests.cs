using AutoFixture;
using Back_End.Data.Repositories;
using Back_End.DTOs.Review;
using Back_End.Models;
using Back_End.Services.impl;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;
using Xunit;

namespace Back_End.Tests.Services
{
    public class ReviewServiceTests
    {
        private readonly Mock<IReviewRepository> _reviewRepositoryMock;
        private readonly Mock<IItemRepository> _itemRepositoryMock;
        private readonly Mock<UserManager<UserModel>> _userManagerMock;
        private readonly ReviewService _reviewService;
        private readonly Fixture _fixture;

        public ReviewServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _reviewRepositoryMock = new Mock<IReviewRepository>();
            _itemRepositoryMock = new Mock<IItemRepository>();

            var userStoreMock = new Mock<IUserStore<UserModel>>();
            _userManagerMock = new Mock<UserManager<UserModel>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _reviewService = new ReviewService(
                _reviewRepositoryMock.Object,
                _itemRepositoryMock.Object,
                _userManagerMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllReviews()
        {
            // Arrange
            var reviews = _fixture.CreateMany<ReviewModel>(3).ToList();
            _reviewRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(reviews);

            // Act
            var result = await _reviewService.GetAllAsync();

            // Assert
            result.Should().HaveCount(3);
            _reviewRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnReview()
        {
            // Arrange
            var review = _fixture.Create<ReviewModel>();
            _reviewRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(review);

            // Act
            var result = await _reviewService.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(review);
            _reviewRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            _reviewRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((ReviewModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _reviewService.GetByIdAsync(1));
        }

        [Fact]
        public async Task CreateAsync_WithValidData_ShouldCreateReview()
        {
            // Arrange
            var item = _fixture.Create<ItemModel>();
            var user = _fixture.Create<UserModel>();
            var createDto = _fixture.Create<CreateReviewRequestDto>();
            var expectedReview = _fixture.Create<ReviewModel>();

            _itemRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(item);
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _reviewRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<ReviewModel>()))
                .ReturnsAsync(expectedReview);


            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", "testuser"),
            }, "TestAuthentication"));

            // Act
            var result = await _reviewService.CreateAsync(1, claimsPrincipal, createDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedReview);
            _reviewRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<ReviewModel>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldUpdateReview()
        {
            // Arrange
            var existingReview = _fixture.Create<ReviewModel>();
            var updateDto = _fixture.Create<UpdateReviewRequestDto>();
            var updatedReview = _fixture.Create<ReviewModel>();

            _reviewRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(existingReview);
            _reviewRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<ReviewModel>(), It.IsAny<UpdateReviewRequestDto>()))
                .ReturnsAsync(updatedReview);

            // Act
            var result = await _reviewService.UpdateAsync(1, updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(updatedReview);
            _reviewRepositoryMock.Verify(x => x.UpdateAsync(existingReview, updateDto), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteReview()
        {
            // Arrange
            var review = _fixture.Create<ReviewModel>();
            _reviewRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(review);
            _reviewRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<ReviewModel>()))
                .ReturnsAsync(review);

            // Act
            var result = await _reviewService.DeleteAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(review);
            _reviewRepositoryMock.Verify(x => x.DeleteAsync(review), Times.Once);
        }
    }
}