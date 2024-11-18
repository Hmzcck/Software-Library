using System.Security.Claims;
using AutoFixture;
using Back_End.Controllers;
using Back_End.DTOs.Review;
using Back_End.Models;
using Back_End.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Back_End.Tests.Controllers
{
    public class ReviewControllerTests
    {
        private readonly Mock<IReviewService> _mockReviewService;
        private readonly ReviewController _controller;
        private readonly Fixture _fixture;

        public ReviewControllerTests()
        {
            _mockReviewService = new Mock<IReviewService>();
            _controller = new ReviewController(_mockReviewService.Object);
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        
        [Fact]
        public async Task GetAll_ReturnsOkResult_WithReviews()
        {
            // Arrange
            var expectedReviews = _fixture.CreateMany<ReviewResponseDto>().ToList();
            _mockReviewService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(expectedReviews);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var reviews = okResult.Value.Should().BeAssignableTo<List<ReviewResponseDto>>().Subject;
            reviews.Should().BeEquivalentTo(expectedReviews);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenReviewExists()
        {
            // Arrange
            var id = _fixture.Create<int>();
            var expectedReview = _fixture.Create<ReviewModel>();
            _mockReviewService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync(expectedReview);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(expectedReview);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WithNewReview()
        {
            // Arrange
            var itemId = _fixture.Create<int>();
            var createDto = _fixture.Create<CreateReviewRequestDto>();
            var createdReview = _fixture.Create<ReviewModel>();
            _mockReviewService.Setup(s => s.CreateAsync(itemId, It.IsAny<ClaimsPrincipal>(), createDto))
                .ReturnsAsync(createdReview);

            // Act
            var result = await _controller.Create(itemId, createDto);

            // Assert
            var createdAtResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtResult.ActionName.Should().Be(nameof(ReviewController.GetById));
            createdAtResult.RouteValues["id"].Should().Be(createdReview.Id);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenUpdateSuccessful()
        {
            // Arrange
            var id = _fixture.Create<int>();
            var updateDto = _fixture.Create<UpdateReviewRequestDto>();
            _mockReviewService.Setup(s => s.UpdateAsync(id, updateDto))
                .ReturnsAsync(new ReviewModel());

            // Act
            var result = await _controller.Update(id, updateDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeleteSuccessful()
        {
            // Arrange
            var id = _fixture.Create<int>();
            _mockReviewService.Setup(s => s.DeleteAsync(id))
                .ReturnsAsync(new ReviewModel());

            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }


        [Fact]
        public async Task GetById_WhenServiceThrowsKeyNotFoundException_ReturnsNotFound()
        {
            // Arrange
            var id = _fixture.Create<int>();
            _mockReviewService.Setup(s => s.GetByIdAsync(id))
                .ThrowsAsync(new KeyNotFoundException("review was not found"));

            // Act
            var result = async () => await _controller.GetById(id);

            // Assert
            await result.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("review was not found");
        }
    }
}