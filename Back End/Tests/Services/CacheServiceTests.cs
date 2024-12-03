using System.Net;
using System.Text;
using AutoFixture;
using Back_End.Services.impl;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using StackExchange.Redis;
using Xunit;

namespace Back_End.Tests.Services
{
    public class CacheServiceTests
    {
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly Mock<IConnectionMultiplexer> _redisMock;
        private readonly CacheService _cacheService;
        private readonly IFixture _fixture;

        public CacheServiceTests()
        {
            _fixture = new Fixture();
            _cacheMock = new Mock<IDistributedCache>();
            _redisMock = new Mock<IConnectionMultiplexer>();
            _cacheService = new CacheService(_redisMock.Object, _cacheMock.Object);
        }

        [Fact]
        public async Task GetAsync_ReturnsDeserializedValue_WhenKeyExists()
        {
            // Arrange
            var key = "test-key";
            var expectedData = _fixture.Create<TestData>();
            var serializedData = JsonConvert.SerializeObject(expectedData);

            _cacheMock.Setup(x => x.GetAsync(key, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Encoding.UTF8.GetBytes(serializedData));

            // Act
            var result = await _cacheService.GetAsync<TestData>(key);

            // Assert
            result.Should().BeEquivalentTo(expectedData);
        }

        [Fact]
        public async Task GetAsync_ReturnsDefault_WhenKeyDoesNotExist()
        {
            // Arrange
            var key = "non-existent-key";
            _cacheMock.Setup(x => x.GetAsync(key, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((byte[])null);

            // Act
            var result = await _cacheService.GetAsync<TestData>(key);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task SetAsync_StoresSerializedValue_WithDefaultExpiry()
        {
            // Arrange
            var key = "test-key";
            var value = _fixture.Create<TestData>();

            // Act
            await _cacheService.SetAsync(key, value);

            // Assert
            _cacheMock.Verify(x => x.SetAsync(
                key,
                It.Is<byte[]>(b =>
                    b != null &&
                    Encoding.UTF8.GetString(b) == JsonConvert.SerializeObject(value)),
                It.Is<DistributedCacheEntryOptions>(o =>
                    o.AbsoluteExpirationRelativeToNow != null &&
                    o.AbsoluteExpirationRelativeToNow.Value.TotalMinutes == 5),
                default
            ), Times.Once);
        }

        [Fact]
        public async Task SetAsync_StoresSerializedValue_WithCustomExpiry()
        {
            // Arrange
            var key = "test-key";
            var value = _fixture.Create<TestData>();
            var expiry = TimeSpan.FromMinutes(10);

            // Act
            await _cacheService.SetAsync(key, value, expiry);

            // Assert
            _cacheMock.Verify(x => x.SetAsync(
                key,
                It.Is<byte[]>(b => 
                    b != null && 
                    Encoding.UTF8.GetString(b) == JsonConvert.SerializeObject(value)),
                It.Is<DistributedCacheEntryOptions>(o =>
                    o.AbsoluteExpirationRelativeToNow == expiry),
                default
            ), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_RemovesKey()
        {
            // Arrange
            var key = "test-key";

            // Act
            await _cacheService.RemoveAsync(key);

            // Assert
            _cacheMock.Verify(x => x.RemoveAsync(key, default), Times.Once);
        }

        [Fact]
        public async Task ClearAsync_DeletesAllMatchingKeys()
        {
            // Arrange
            var keyPrefix = "test_";
            var mockServer = new Mock<IServer>();
            var mockDatabase = new Mock<IDatabase>();
            var mockEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6379);
            var keys = new[] {
        new RedisKey("test_1"),
        new RedisKey("test_2")
    };

            _redisMock.Setup(x => x.GetEndPoints(It.IsAny<bool>()))
                .Returns(new EndPoint[] { mockEndPoint });
            _redisMock.Setup(x => x.GetServer(mockEndPoint, It.IsAny<object>()))
                .Returns(mockServer.Object);
            _redisMock.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(mockDatabase.Object);
            mockServer.Setup(x => x.Keys(
                It.IsAny<int>(),              // database
                $"{keyPrefix}*",              // pattern
                It.IsAny<int>(),              // pageSize
                It.IsAny<long>(),             // cursor
                It.IsAny<int>(),              // pageOffset
                CommandFlags.None              // flags
            )).Returns(keys);
            mockDatabase.Setup(x => x.KeyDeleteAsync(It.IsAny<RedisKey>(), CommandFlags.None))
                .ReturnsAsync(true);

            // Act
            await _cacheService.ClearAsync(keyPrefix);

            // Assert
            mockDatabase.Verify(x => x.KeyDeleteAsync(It.IsAny<RedisKey>(), CommandFlags.None),
                Times.Exactly(keys.Length));
        }

        private class TestData
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }
    }
}