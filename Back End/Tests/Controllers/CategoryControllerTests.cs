using AutoFixture;
using Back_End.Controllers;
using Back_End.DTOs.Category;
using Back_End.Models;
using Back_End.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Back_End.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly CategoryController _controller;
        private readonly IFixture _fixture;

        public CategoryControllerTests()
        {
            _fixture = new Fixture();

            // Configure AutoFixture to handle circular references
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _categoryServiceMock = new Mock<ICategoryService>();
            _controller = new CategoryController(_categoryServiceMock.Object);
        }
        [Fact]
        public async Task GetAll_ReturnsOkResult_WithCategories()
        {
            // Arrange
            var expectedCategories = _fixture.CreateMany<CategoryResponseDto>();
            _categoryServiceMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(expectedCategories.ToList());

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var categories = okResult.Value.Should().BeAssignableTo<IEnumerable<CategoryResponseDto>>().Subject;
            categories.Should().BeEquivalentTo(expectedCategories);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenCategoryExists()
        {
            // Arrange
            var categoryId = _fixture.Create<int>();
            var expectedCategory = _fixture.Create<CategoryResponseDto>();
            _categoryServiceMock.Setup(x => x.GetByIdAsync(categoryId))
                .ReturnsAsync(expectedCategory);

            // Act
            var result = await _controller.GetById(categoryId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var category = okResult.Value.Should().BeOfType<CategoryResponseDto>().Subject;
            category.Should().BeEquivalentTo(expectedCategory);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WhenValidRequest()
        {
            // Arrange
            var createDto = _fixture.Create<CreateCategoryRequestDto>();
            var createdCategory = _fixture.Create<CategoryModel>();
            var expectedResponse = _fixture.Create<CategoryResponseDto>();

            _categoryServiceMock.Setup(x => x.CreateAsync(createDto))
                .ReturnsAsync(createdCategory);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdAtResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtResult.ActionName.Should().Be(nameof(CategoryController.GetById));
            createdAtResult.RouteValues["id"].Should().Be(createdCategory.Id);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenValidRequest()
        {
            // Arrange
            var categoryId = _fixture.Create<int>();
            var updateDto = _fixture.Create<UpdateCategoryRequestDto>();
            var updatedCategory = _fixture.Create<CategoryModel>();

            _categoryServiceMock.Setup(x => x.UpdateAsync(categoryId, updateDto))
                .ReturnsAsync(updatedCategory);

            // Act
            var result = await _controller.Update(categoryId, updateDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenCategoryExists()
        {
            // Arrange
            var categoryId = _fixture.Create<int>();
            var deletedCategory = _fixture.Create<CategoryModel>();

            _categoryServiceMock.Setup(x => x.DeleteAsync(categoryId))
                .ReturnsAsync(deletedCategory);

            // Act
            var result = await _controller.Delete(categoryId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task GetById_ThrowsKeyNotFoundException_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryId = _fixture.Create<int>();
            _categoryServiceMock.Setup(x => x.GetByIdAsync(categoryId))
                .ThrowsAsync(new KeyNotFoundException("category was not found"));

            // Act
            var result = async () => await _controller.GetById(categoryId);

            // Assert
            await result.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("category was not found");
        }
    }
}