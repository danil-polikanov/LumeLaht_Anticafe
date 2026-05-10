using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using UserService.Application.Services;
using UserService.Core.Entities;
using UserService.Core.Interfaces;
using Xunit;

namespace UserService.Tests.Services
{
    public class UserAppServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly UserService.Application.Services.UserService _service;

        public UserAppServiceTests()
        {
            _uowMock.Setup(u => u.Users).Returns(_userRepoMock.Object);
            _service = new UserService.Application.Services.UserService(
                _uowMock.Object,
                NullLogger<UserService.Application.Services.UserService>.Instance);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUserResponse_WhenFound()
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = "Test", LastName = "User",
                Email = "found@test.com",
                PasswordHash = "irrelevant", Role = "Client",
            };
            _userRepoMock.Setup(r => r.GetByIdAsync(user.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var result = await _service.GetUserByIdAsync(user.UserId, CancellationToken.None);

            result.Should().NotBeNull();
            result!.UserId.Should().Be(user.UserId);
            result.Email.Should().Be(user.Email);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsNull_WhenNotFound()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            var result = await _service.GetUserByIdAsync(Guid.NewGuid(), CancellationToken.None);

            result.Should().BeNull();
        }
    }
}
