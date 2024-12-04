using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using AutoFixture;
using Back_End.Models;
using Back_End.Services.impl;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Back_End.Tests.Services
{
    public class TokenServiceTests
    {
        private readonly Mock<IConfiguration> _configMock;
        private readonly TokenService _tokenService;
        private readonly IFixture _fixture;
        private readonly string _testSecretKey;
        private readonly string _testIssuer;
        private readonly string _testAudience;

         public TokenServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            
            // Key must be at least 64 bytes (512 bits) for HMACSHA512
            _testSecretKey = "ThisIsAVeryLongSecretKeyThatIsAtLeast64BytesLongForHS512SigningAlgorithm123!@#$%^&*()";
            _testIssuer = "TestIssuer";
            _testAudience = "TestAudience";

            _configMock = new Mock<IConfiguration>();
            _configMock.Setup(x => x["JWT:SigningKey"]).Returns(_testSecretKey);
            _configMock.Setup(x => x["JWT:Issuer"]).Returns(_testIssuer);
            _configMock.Setup(x => x["JWT:Audience"]).Returns(_testAudience);

            _tokenService = new TokenService(_configMock.Object);
        }

        [Fact]
        public async Task CreateTokenAsync_ShouldCreateValidToken()
        {
            // Arrange
            var user = _fixture.Create<UserModel>();

            // Act
            var token = await _tokenService.CreateTokenAsync(user);

            // Assert
            token.Should().NotBeNullOrEmpty();
            var jwtHandler = new JwtSecurityTokenHandler();
            var decodedToken = jwtHandler.ReadJwtToken(token);
            
            decodedToken.Should().NotBeNull();
            decodedToken.Header.Alg.Should().Be("HS512");
        }

        [Fact]
        public async Task CreateTokenAsync_ShouldContainCorrectClaims()
        {
            // Arrange
            var user = _fixture.Create<UserModel>();

            // Act
            var token = await _tokenService.CreateTokenAsync(user);

            // Assert
            var jwtHandler = new JwtSecurityTokenHandler();
            var decodedToken = jwtHandler.ReadJwtToken(token);

            decodedToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);
            decodedToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.GivenName && c.Value == user.UserName);
        }

        [Fact]
        public async Task CreateTokenAsync_ShouldHaveCorrectExpirationTime()
        {
            // Arrange
            var user = _fixture.Create<UserModel>();

            // Act
            var token = await _tokenService.CreateTokenAsync(user);

            // Assert
            var jwtHandler = new JwtSecurityTokenHandler();
            var decodedToken = jwtHandler.ReadJwtToken(token);

            decodedToken.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddDays(7), TimeSpan.FromMinutes(1));
        }

        [Fact]
        public async Task CreateTokenAsync_ShouldHaveCorrectIssuerAndAudience()
        {
            // Arrange
            var user = _fixture.Create<UserModel>();

            // Act
            var token = await _tokenService.CreateTokenAsync(user);

            // Assert
            var jwtHandler = new JwtSecurityTokenHandler();
            var decodedToken = jwtHandler.ReadJwtToken(token);

            decodedToken.Issuer.Should().Be(_testIssuer);
            decodedToken.Audiences.Should().Contain(_testAudience);
        }
    }
}