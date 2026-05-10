using System.Security.Claims;
using BookingService.API.Controllers;
using BookingService.Application.Dto.Booking;
using BookingService.Application.IServices;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BookingService.Tests.Controllers
{
    public class BookingControllerTests
    {
        private readonly Mock<IBookingService> _serviceMock = new();
        private readonly BookingController _controller;
        private readonly Guid _userId = Guid.NewGuid();

        public BookingControllerTests()
        {
            _controller = new BookingController(_serviceMock.Object);
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, _userId.ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) },
            };
        }

        [Fact]
        public async Task Create_Returns200_WithBookingResponse()
        {
            var request = new CreateBookingRequest { RoomId = Guid.NewGuid(), StartTime = DateTime.UtcNow.AddHours(2) };
            var expected = new BookingResponse { BookingId = Guid.NewGuid(), Status = "Confirmed" };
            _serviceMock.Setup(s => s.CreateBookingAsync(_userId, request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var result = await _controller.Create(request, CancellationToken.None);

            var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().Be(expected);
        }

        [Fact]
        public async Task GetMyBookings_Returns200_WithCurrentUsersBookingsOnly()
        {
            var bookings = new List<BookingResponse> { new() { BookingId = Guid.NewGuid() } };
            _serviceMock.Setup(s => s.GetUserBookingsAsync(_userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookings);

            var result = await _controller.GetMyBookings(CancellationToken.None);

            var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().BeEquivalentTo(bookings);
            _serviceMock.Verify(s => s.GetUserBookingsAsync(_userId, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Cancel_Returns204NoContent_WhenServiceSucceeds()
        {
            var bookingId = Guid.NewGuid();
            _serviceMock.Setup(s => s.CancelBookingAsync(_userId, bookingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await _controller.Cancel(bookingId, CancellationToken.None);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task GetRoomBookings_Returns200_WithListForGivenDate()
        {
            var roomId = Guid.NewGuid();
            var date = DateTime.UtcNow.Date;
            var bookings = new List<BookingResponse> { new() { BookingId = Guid.NewGuid(), RoomId = roomId } };
            _serviceMock.Setup(s => s.GetRoomBookingsAsync(roomId, date, It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookings);

            var result = await _controller.GetRoomBookings(roomId, date, CancellationToken.None);

            var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().BeEquivalentTo(bookings);
        }

        [Fact]
        public async Task Create_PropagatesServiceException()
        {
            var request = new CreateBookingRequest { RoomId = Guid.NewGuid(), StartTime = DateTime.UtcNow.AddHours(2) };
            _serviceMock.Setup(s => s.CreateBookingAsync(_userId, request, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("This time slot is already booked"));

            var act = () => _controller.Create(request, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("This time slot is already booked");
        }
    }
}
