using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.API.Controllers;
using UserService.Application.Dto;
using UserService.Application.IServices;
using Xunit;

namespace UserService.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _serviceMock = new();
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _controller = new UsersController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetUserById_Returns200_WithUser_WhenFound()
        {
            var userId = Guid.NewGuid();
            var expected = new UserResponse
            {
                UserId = userId,
                FirstName = "Test", LastName = "User",
                Email = "test@test.com",
            };
            _serviceMock.Setup(s => s.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var result = await _controller.GetUserById(userId, CancellationToken.None);

            var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().Be(expected);
        }

        [Fact]
        public async Task GetUserById_Returns404_WhenNotFound()
        {
            _serviceMock.Setup(s => s.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserResponse?)null);

            var result = await _controller.GetUserById(Guid.NewGuid(), CancellationToken.None);

            result.Result.Should().BeOfType<NotFoundResult>();
        }
    }
}
