using System.Threading.Tasks;
using AutoFixture;
using Back_End.DTOs.User;
using Back_End.Models;
using Back_End.Services.impl;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Back_End.Services;
using MockQueryable.Moq;

namespace Back_End.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<UserModel>> _userManagerMock;
        private readonly Mock<SignInManager<UserModel>> _signInManagerMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly AuthService _authService;
        private readonly Fixture _fixture;

        public AuthServiceTests()
        {
            _fixture = new Fixture();

            var userStoreMock = new Mock<IUserStore<UserModel>>();
            _userManagerMock = new Mock<UserManager<UserModel>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextAccessorMock = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<UserModel>>();
            _signInManagerMock = new Mock<SignInManager<UserModel>>(
                _userManagerMock.Object,
                contextAccessorMock.Object,
                userPrincipalFactoryMock.Object,
                null, null, null, null);

            _tokenServiceMock = new Mock<ITokenService>();

            _authService = new AuthService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _tokenServiceMock.Object);
        }

        [Fact]
        public async Task RegisterUserAsync_WhenSuccessful_ReturnsSuccessResult()
        {
            // Arrange
            var registerDto = _fixture.Create<RegisterDTO>();
            var token = "test-token";

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<UserModel>(), registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<UserModel>(), "User"))
                .ReturnsAsync(IdentityResult.Success);

            _tokenServiceMock.Setup(x => x.CreateTokenAsync(It.IsAny<UserModel>()))
                .ReturnsAsync(token);

            // Act
            var result = await _authService.RegisterUserAsync(registerDto);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.NewUser.Should().NotBeNull();
            result.NewUser.Token.Should().Be(token);
        }

        [Fact]
        public async Task RegisterUserAsync_WhenUserCreationFails_ReturnsFailureResult()
        {
            // Arrange
            var registerDto = _fixture.Create<RegisterDTO>();
            var errors = new[] { new IdentityError { Description = "Error" } };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<UserModel>(), registerDto.Password))
                .ReturnsAsync(IdentityResult.Failed(errors));

            // Act
            var result = await _authService.RegisterUserAsync(registerDto);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }


        [Fact]
        public async Task LoginUserAsync_WhenSuccessful_ReturnsSuccessResult()
        {
            // Arrange
            var loginDto = _fixture.Create<LoginDTO>();
            var user = new UserModel { UserName = loginDto.Username };
            var token = "test-token";

            var users = new List<UserModel> { user }.AsQueryable();
            var mockDbSet = users.BuildMockDbSet();

            _userManagerMock.Setup(x => x.Users)
                .Returns(mockDbSet.Object);

            _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, loginDto.Password, false))
                .ReturnsAsync(SignInResult.Success);

            _tokenServiceMock.Setup(x => x.CreateTokenAsync(user))
                .ReturnsAsync(token);

            // Act
            var result = await _authService.LoginUserAsync(loginDto);

            // Assert
            result.Success.Should().BeTrue();
            result.Token.Should().Be(token);
            result.UserName.Should().Be(user.UserName);
        }

        [Fact]
        public async Task LoginUserAsync_WhenUserNotFound_ReturnsFailureResult()
        {
            // Arrange
            var loginDto = _fixture.Create<LoginDTO>();

            var users = new List<UserModel>();
            var mockDbSet = users.AsQueryable().BuildMockDbSet();

            _userManagerMock.Setup(x => x.Users)
                .Returns(mockDbSet.Object);

            // Act
            var result = await _authService.LoginUserAsync(loginDto);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain("Invalid credentials");
        }

        [Fact]
        public async Task LoginUserAsync_WhenPasswordInvalid_ReturnsFailureResult()
        {
            // Arrange
            var loginDto = _fixture.Create<LoginDTO>();
            var user = new UserModel { UserName = loginDto.Username };

            var users = new List<UserModel> { user };
            var mockDbSet = users.AsQueryable().BuildMockDbSet();

            _userManagerMock.Setup(x => x.Users)
                .Returns(mockDbSet.Object);

            _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, loginDto.Password, false))
                .ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await _authService.LoginUserAsync(loginDto);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain("Invalid credentials");
        }
    }
}