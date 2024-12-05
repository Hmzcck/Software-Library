using AutoFixture;
using Back_End.Data;
using Back_End.Data.Repositories.impl;
using Back_End.DTOs.Review;
using Back_End.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Back_End.Tests.Repositories
{
    public class ReviewRepositoryTests : IDisposable
    {
        private readonly ApplicationDBContext _context;
        private readonly ReviewRepository _repository;
        private readonly Fixture _fixture;

        public ReviewRepositoryTests()
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
            _repository = new ReviewRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllReviews()
        {
            // Arrange
            var reviews = _fixture.CreateMany<ReviewModel>(3).ToList();

            await _context.Reviews.AddRangeAsync(reviews);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(reviews);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnReview_WhenExists()
        {
            // Arrange
            var review = _fixture.Create<ReviewModel>();
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(review.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(review);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddNewReview()
        {
            // Arrange
            var review = _fixture.Create<ReviewModel>();

            // Act
            var result = await _repository.CreateAsync(review);

            // Assert
            var savedReview = await _context.Reviews.FindAsync(result.Id);
            savedReview.Should().NotBeNull();
            result.Should().BeEquivalentTo(review);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingReview()
        {
            // Arrange
            var existingReview = _fixture.Create<ReviewModel>();
            await _context.Reviews.AddAsync(existingReview);
            await _context.SaveChangesAsync();

            var updateDto = _fixture.Create<UpdateReviewRequestDto>();

            // Act
            var result = await _repository.UpdateAsync(existingReview, updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(updateDto.Name);
            result.Rating.Should().Be(updateDto.Rating);
            result.Comment.Should().Be(updateDto.Comment);

            var updatedReview = await _context.Reviews.FindAsync(existingReview.Id);
            updatedReview.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveReview()
        {
            // Arrange
            var review = _fixture.Create<ReviewModel>();
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.DeleteAsync(review);

            // Assert
            result.Should().BeEquivalentTo(review);
            var deletedReview = await _context.Reviews.FindAsync(review.Id);
            deletedReview.Should().BeNull();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}