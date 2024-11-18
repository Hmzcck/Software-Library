using AutoFixture;
using Back_End.Controllers;
using Back_End.DTOs.Item;
using Back_End.Models;
using Back_End.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Back_End.Tests.Controllers
{
    public class ItemControllerTests
    {
        private readonly Mock<IItemService> _mockItemService;
        private readonly ItemController _controller;
        private readonly Fixture _fixture;
        private readonly Mock<ICacheService> _mockCacheService;

        public ItemControllerTests()
        {
            _mockItemService = new Mock<IItemService>();
            _mockCacheService = new Mock<ICacheService>();
            _controller = new ItemController(_mockItemService.Object, _mockCacheService.Object);
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        [Fact]
        public async Task GetAll_ReturnsOkResult_WithItems()
        {
            // Arrange
            var filterDto = _fixture.Create<ItemFilterDto>();
            var expectedResponse = _fixture.Create<PaginatedResponse<ItemResponseDto>>();
            _mockItemService.Setup(s => s.GetAllAsync(It.IsAny<ItemFilterDto>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAll(filterDto, "test");

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var items = okResult.Value.Should().BeAssignableTo<PaginatedResponse<ItemResponseDto>>().Subject;
            items.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenItemExists()
        {
            // Arrange
            var id = _fixture.Create<int>();
            var expectedItem = _fixture.Create<ItemResponseDto>();
            _mockItemService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync(expectedItem);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var item = okResult.Value.Should().BeAssignableTo<ItemResponseDto>().Subject;
            item.Should().BeEquivalentTo(expectedItem);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WithNewItem()
        {
            // Arrange
            var createDto = _fixture.Create<CreateItemRequestDto>();
            var createdItem = _fixture.Create<ItemModel>();
            _mockItemService.Setup(s => s.CreateAsync(createDto))
                .ReturnsAsync(createdItem);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdAtResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtResult.ActionName.Should().Be(nameof(ItemController.GetById));
            createdAtResult.RouteValues["id"].Should().Be(createdItem.Id);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenUpdateSuccessful()
        {
            // Arrange
            var id = _fixture.Create<int>();
            var updateDto = _fixture.Create<UpdateItemRequestDto>();
            _mockItemService.Setup(s => s.UpdateAsync(id, updateDto))
                .ReturnsAsync(new ItemModel());

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
            _mockItemService.Setup(s => s.DeleteAsync(id))
                .ReturnsAsync(new ItemModel());

            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task AddCategory_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var addCategoryDto = _fixture.Create<AddCategoryRequestDto>();
            _mockItemService.Setup(s => s.AddCategoryAsync(addCategoryDto))
                .ReturnsAsync(new ItemModel());

            // Act
            var result = await _controller.AddCategory(addCategoryDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void TestError_ThrowsKeyNotFoundException()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _controller.TestError());
        }

        [Fact]
        public async Task GetAll_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await _controller.GetAll(new ItemFilterDto(), "test");

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetById_WhenServiceThrowsKeyNotFoundException_ReturnsNotFound()
        {
            // Arrange
            var id = _fixture.Create<int>();
            _mockItemService.Setup(s => s.GetByIdAsync(id))
                .ThrowsAsync(new KeyNotFoundException("item was not found"));

            // Act
            var result = async () => await _controller.GetById(id);

            // Assert
            await result.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("item was not found");
        }
    }
}