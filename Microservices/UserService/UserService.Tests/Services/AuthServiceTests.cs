using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using UserService.Application.Dto.Auth;
using UserService.Application.Services;
using UserService.Application.Settings;
using UserService.Core.Entities;
using UserService.Core.Interfaces;
using Xunit;

namespace UserService.Tests.Services
{
    /// <summary>
    /// Microservices UserService.AuthService — should behave identically to
    /// the Separated/Monolith AuthService. Tests pin contract parity so the
    /// two paths cannot drift (matters because the bcrypt cost-factor is the
    /// dominant CPU work in the benchmark; a divergence here would invalidate
    /// the architecture comparison).
    /// </summary>
    public class AuthServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly AuthService _service;

        private static readonly JwtSettings TestJwt = new()
        {
            SecretKey = "test-secret-key-for-jwt-signing-min-32-chars",
            Issuer = "LumeLahtTests",
            Audience = "LumeLahtClients",
            ExpirationMinutes = 60,
        };

        public AuthServiceTests()
        {
            _uowMock.Setup(u => u.Users).Returns(_userRepoMock.Object);
            _service = new AuthService(
                _uowMock.Object,
                Options.Create(TestJwt),
                NullLogger<AuthService>.Instance);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsAuthResponse_WhenEmailIsNew()
        {
            var request = new RegisterRequest
            {
                Email = "new@test.com",
                Password = "TestPass123!",
                FirstName = "Test",
                LastName = "User",
                Phone = "+37255500000",
            };
            _userRepoMock.Setup(r => r.GetByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            var result = await _service.RegisterAsync(request, CancellationToken.None);

            result.Should().NotBeNull();
            result.Email.Should().Be("new@test.com");
            result.Role.Should().Be("Client");
            result.Token.Should().NotBeNullOrWhiteSpace();
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_HashesPasswordWithBcrypt()
        {
            var request = new RegisterRequest
            {
                Email = "hash@test.com",
                Password = "PlaintextPwd",
                FirstName = "T", LastName = "U",
            };
            User? captured = null;
            _userRepoMock.Setup(r => r.GetByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);
            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback<User, CancellationToken>((u, _) => captured = u);

            await _service.RegisterAsync(request, CancellationToken.None);

            captured.Should().NotBeNull();
            captured!.PasswordHash.Should().NotBe("PlaintextPwd");
            BCrypt.Net.BCrypt.Verify("PlaintextPwd", captured.PasswordHash).Should().BeTrue();
        }

        [Fact]
        public async Task RegisterAsync_Throws_WhenEmailAlreadyExists()
        {
            var request = new RegisterRequest
            {
                Email = "existing@test.com",
                Password = "TestPass123!",
                FirstName = "T", LastName = "U",
            };
            _userRepoMock.Setup(r => r.GetByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new User { Email = request.Email });

            var act = () => _service.RegisterAsync(request, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("User with this email already exists");
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task LoginAsync_ReturnsAuthResponse_WhenCredentialsValid()
        {
            var password = "MyPassword!";
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = "valid@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                FirstName = "Test", LastName = "User",
                Role = "Client",
            };
            _userRepoMock.Setup(r => r.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var result = await _service.LoginAsync(
                new LoginRequest { Email = user.Email, Password = password },
                CancellationToken.None);

            result.Should().NotBeNull();
            result.UserId.Should().Be(user.UserId);
            result.Email.Should().Be(user.Email);
            result.Token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task LoginAsync_Throws_WhenUserNotFound()
        {
            _userRepoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            var act = () => _service.LoginAsync(
                new LoginRequest { Email = "nope@test.com", Password = "anything" },
                CancellationToken.None);

            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Invalid email or password");
        }

        [Fact]
        public async Task LoginAsync_Throws_WhenPasswordWrong()
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = "wrongpw@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("RealPassword"),
                FirstName = "T", LastName = "U", Role = "Client",
            };
            _userRepoMock.Setup(r => r.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var act = () => _service.LoginAsync(
                new LoginRequest { Email = user.Email, Password = "WrongGuess" },
                CancellationToken.None);

            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Invalid email or password");
        }
    }
}
