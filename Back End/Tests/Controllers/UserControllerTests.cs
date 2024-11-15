using AutoFixture;
using Back_End.Controllers;
using Back_End.DTOs.User;
using Back_End.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Back_End.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly UserController _controller;
        private readonly IFixture _fixture;

        public UserControllerTests()
        {
            _fixture = new Fixture();
            _authServiceMock = new Mock<IAuthService>();
            _controller = new UserController(_authServiceMock.Object);
        }

        [Fact]
        public async Task Register_ValidData_ReturnsOkResult()
        {
            // Arrange
            var registerDto = _fixture.Create<RegisterDTO>();
            var authResult = new RegisterResultDTO
            {
                Succeeded = true,
                NewUser = _fixture.Create<NewUserDTO>()
            };

            _authServiceMock
                .Setup(x => x.RegisterUserAsync(It.IsAny<RegisterDTO>()))
                .ReturnsAsync(authResult);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(authResult.NewUser);
            _authServiceMock.Verify(x => x.RegisterUserAsync(registerDto), Times.Once);
        }

        [Fact]
        public async Task Register_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = _fixture.Create<RegisterDTO>();
            _controller.ModelState.AddModelError("error", "test error");

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            _authServiceMock.Verify(x => x.RegisterUserAsync(It.IsAny<RegisterDTO>()), Times.Never);
        }

        [Fact]
        public async Task Register_RegistrationFailed_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = _fixture.Create<RegisterDTO>();
            var authResult = new RegisterResultDTO
            {
                Succeeded = false,
                Errors = new[] { new IdentityError { Description = "Error" } }
            };

            _authServiceMock
                .Setup(x => x.RegisterUserAsync(It.IsAny<RegisterDTO>()))
                .ReturnsAsync(authResult);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be(authResult.Errors);
        }

        [Fact]
        public async Task Login_ValidData_ReturnsOkResult()
        {
            // Arrange
            var loginDto = _fixture.Create<LoginDTO>();
            var loginResult = new LoginResultDTO { Success = true };

            _authServiceMock
                .Setup(x => x.LoginUserAsync(It.IsAny<LoginDTO>()))
                .ReturnsAsync(loginResult);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(loginResult);
            _authServiceMock.Verify(x => x.LoginUserAsync(loginDto), Times.Once);
        }

        [Fact]
        public async Task Login_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var loginDto = _fixture.Create<LoginDTO>();
            _controller.ModelState.AddModelError("error", "test error");

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            _authServiceMock.Verify(x => x.LoginUserAsync(It.IsAny<LoginDTO>()), Times.Never);
        }

        [Fact]
        public async Task Login_LoginFailed_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = _fixture.Create<LoginDTO>();
            var loginResult = new LoginResultDTO { Success = false };

            _authServiceMock
                .Setup(x => x.LoginUserAsync(It.IsAny<LoginDTO>()))
                .ReturnsAsync(loginResult);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
            _authServiceMock.Verify(x => x.LoginUserAsync(loginDto), Times.Once);
        }
    }
}