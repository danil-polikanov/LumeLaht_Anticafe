using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.API.Controllers;
using UserService.Application.Dto.Auth;
using UserService.Application.IServices;
using Xunit;

namespace UserService.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _serviceMock = new();
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _controller = new AuthController(_serviceMock.Object);
        }

        [Fact]
        public async Task Register_Returns200_WithAuthResponse_WhenServiceSucceeds()
        {
            var request = new RegisterRequest
            {
                Email = "new@test.com",
                Password = "TestPass123!",
                FirstName = "T", LastName = "U",
            };
            var expected = new AuthResponse
            {
                Token = "jwt.payload.signature",
                UserId = Guid.NewGuid(),
                Email = request.Email,
                Role = "Client",
            };
            _serviceMock.Setup(s => s.RegisterAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var result = await _controller.Register(request, CancellationToken.None);

            var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().Be(expected);
        }

        [Fact]
        public async Task Register_PropagatesException_WhenEmailExists()
        {
            var request = new RegisterRequest { Email = "dup@test.com", Password = "x", FirstName = "T", LastName = "U" };
            _serviceMock.Setup(s => s.RegisterAsync(request, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("User with this email already exists"));

            var act = () => _controller.Register(request, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task Login_Returns200_WithAuthResponse_WhenCredentialsValid()
        {
            var request = new LoginRequest { Email = "valid@test.com", Password = "good" };
            var expected = new AuthResponse
            {
                Token = "jwt.payload.signature",
                UserId = Guid.NewGuid(),
                Email = request.Email,
                Role = "Client",
            };
            _serviceMock.Setup(s => s.LoginAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var result = await _controller.Login(request, CancellationToken.None);

            var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().Be(expected);
        }

        [Fact]
        public async Task Login_PropagatesUnauthorized_WhenServiceRejects()
        {
            var request = new LoginRequest { Email = "wrong@test.com", Password = "bad" };
            _serviceMock.Setup(s => s.LoginAsync(request, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UnauthorizedAccessException("Invalid email or password"));

            var act = () => _controller.Login(request, CancellationToken.None);

            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}
