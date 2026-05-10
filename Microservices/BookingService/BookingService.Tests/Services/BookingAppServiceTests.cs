using BookingService.Application.Dto;
using BookingService.Application.Dto.Booking;
using BookingService.Application.IServices;
using BookingService.Application.Services;
using BookingService.Core.Entities;
using BookingService.Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace BookingService.Tests.Services
{
    /// <summary>
    /// Microservices BookingAppService — same business contract as the
    /// Separated/Monolith BookingService, but Room is fetched over HTTP via
    /// IRoomApiClient instead of an in-process repository. These tests pin
    /// down the contract so the two paths cannot drift again (regression
    /// guard for the HasConflictAsync + room.Status fixes).
    /// </summary>
    public class BookingAppServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IBookingRepository> _bookingRepoMock = new();
        private readonly Mock<IRoomApiClient> _roomApiMock = new();
        private readonly BookingAppService _service;

        public BookingAppServiceTests()
        {
            _uowMock.Setup(u => u.Bookings).Returns(_bookingRepoMock.Object);
            _service = new BookingAppService(
                _uowMock.Object,
                _roomApiMock.Object,
                NullLogger<BookingAppService>.Instance);
        }

        private static DateTime FutureSlot() =>
            DateTime.UtcNow.AddHours(2).Date.AddHours(DateTime.UtcNow.Hour + 2);

        private static RoomDto AvailableRoom(Guid id) => new()
        {
            RoomId = id,
            Name = "Test Room",
            PricePerHour = 5m,
            Status = "Available",
        };

        #region CreateBookingAsync

        [Fact]
        public async Task CreateBookingAsync_ReturnsResponse_WhenSlotIsFree()
        {
            var roomId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var request = new CreateBookingRequest { RoomId = roomId, StartTime = FutureSlot() };

            _roomApiMock.Setup(c => c.GetRoomByIdAsync(roomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(AvailableRoom(roomId));
            _bookingRepoMock.Setup(r => r.HasConflictAsync(roomId,
                    It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var result = await _service.CreateBookingAsync(userId, request, CancellationToken.None);

            result.Should().NotBeNull();
            result.RoomId.Should().Be(roomId);
            result.RoomName.Should().Be("Test Room");
            result.Status.Should().Be("Confirmed");
            result.EndTime.Should().Be(result.StartTime.AddHours(1));
            _bookingRepoMock.Verify(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateBookingAsync_Throws_WhenRoomServiceReturnsNull()
        {
            // Simulates RoomService being unreachable or returning 404.
            _roomApiMock.Setup(c => c.GetRoomByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RoomDto?)null);

            var request = new CreateBookingRequest { RoomId = Guid.NewGuid(), StartTime = FutureSlot() };
            var act = () => _service.CreateBookingAsync(Guid.NewGuid(), request, CancellationToken.None);

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Room not found");
        }

        [Fact]
        public async Task CreateBookingAsync_Throws_WhenRoomUnavailable()
        {
            // Validates the room.Status check that was missing before today's fix.
            var roomId = Guid.NewGuid();
            _roomApiMock.Setup(c => c.GetRoomByIdAsync(roomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RoomDto
                {
                    RoomId = roomId, Name = "Closed Room",
                    PricePerHour = 5m, Status = "Maintenance",
                });

            var request = new CreateBookingRequest { RoomId = roomId, StartTime = FutureSlot() };
            var act = () => _service.CreateBookingAsync(Guid.NewGuid(), request, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Room is not available");
            _bookingRepoMock.Verify(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateBookingAsync_Throws_WhenStartInPast()
        {
            var roomId = Guid.NewGuid();
            _roomApiMock.Setup(c => c.GetRoomByIdAsync(roomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(AvailableRoom(roomId));

            var request = new CreateBookingRequest
            {
                RoomId = roomId,
                StartTime = DateTime.UtcNow.AddHours(-2),
            };
            var act = () => _service.CreateBookingAsync(Guid.NewGuid(), request, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Cannot book a slot in the past");
        }

        [Fact]
        public async Task CreateBookingAsync_Throws_WhenSlotConflicts()
        {
            // Validates the HasConflictAsync repository call that was missing before today's fix.
            var roomId = Guid.NewGuid();
            _roomApiMock.Setup(c => c.GetRoomByIdAsync(roomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(AvailableRoom(roomId));
            _bookingRepoMock.Setup(r => r.HasConflictAsync(roomId,
                    It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var request = new CreateBookingRequest { RoomId = roomId, StartTime = FutureSlot() };
            var act = () => _service.CreateBookingAsync(Guid.NewGuid(), request, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("This time slot is already booked");
            _bookingRepoMock.Verify(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        #endregion

        #region GetUserBookingsAsync

        [Fact]
        public async Task GetUserBookingsAsync_ReturnsBookings_OrderedByStartTimeDescending()
        {
            var userId = Guid.NewGuid();
            var roomA = Guid.NewGuid();
            var roomB = Guid.NewGuid();
            var older = new Booking
            {
                BookingId = Guid.NewGuid(), RoomId = roomA, UserId = userId,
                StartTime = DateTime.UtcNow.AddHours(2), EndTime = DateTime.UtcNow.AddHours(3),
                TotalPrice = 5m, Status = "Confirmed",
            };
            var newer = new Booking
            {
                BookingId = Guid.NewGuid(), RoomId = roomB, UserId = userId,
                StartTime = DateTime.UtcNow.AddHours(10), EndTime = DateTime.UtcNow.AddHours(11),
                TotalPrice = 10m, Status = "Confirmed",
            };
            _bookingRepoMock.Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Booking> { older, newer });
            _roomApiMock.Setup(c => c.GetRoomByIdAsync(roomA, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RoomDto { RoomId = roomA, Name = "Room Alpha", PricePerHour = 5m, Status = "Available" });
            _roomApiMock.Setup(c => c.GetRoomByIdAsync(roomB, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RoomDto { RoomId = roomB, Name = "Room Beta", PricePerHour = 10m, Status = "Available" });

            var result = await _service.GetUserBookingsAsync(userId, CancellationToken.None);

            result.Should().HaveCount(2);
            result[0].BookingId.Should().Be(newer.BookingId);
            result[0].RoomName.Should().Be("Room Beta");
            result[1].BookingId.Should().Be(older.BookingId);
            result[1].RoomName.Should().Be("Room Alpha");
        }

        #endregion

        #region CancelBookingAsync

        [Fact]
        public async Task CancelBookingAsync_ReturnsTrue_WhenOwnerCancelsConfirmed()
        {
            var userId = Guid.NewGuid();
            var booking = new Booking
            {
                BookingId = Guid.NewGuid(), UserId = userId, RoomId = Guid.NewGuid(),
                StartTime = DateTime.UtcNow.AddHours(5), EndTime = DateTime.UtcNow.AddHours(6),
                TotalPrice = 5m, Status = "Confirmed",
            };
            _bookingRepoMock.Setup(r => r.GetByIdAsync(booking.BookingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);

            var result = await _service.CancelBookingAsync(userId, booking.BookingId, CancellationToken.None);

            result.Should().BeTrue();
            booking.Status.Should().Be("Cancelled");
            _bookingRepoMock.Verify(r => r.UpdateAsync(booking, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CancelBookingAsync_Throws_WhenBookingNotFound()
        {
            _bookingRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Booking?)null);

            var act = () => _service.CancelBookingAsync(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Booking not found");
        }

        [Fact]
        public async Task CancelBookingAsync_Throws_WhenUserIsNotOwner()
        {
            var ownerId = Guid.NewGuid();
            var otherId = Guid.NewGuid();
            var booking = new Booking
            {
                BookingId = Guid.NewGuid(), UserId = ownerId, RoomId = Guid.NewGuid(),
                StartTime = DateTime.UtcNow.AddHours(5), EndTime = DateTime.UtcNow.AddHours(6),
                TotalPrice = 5m, Status = "Confirmed",
            };
            _bookingRepoMock.Setup(r => r.GetByIdAsync(booking.BookingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);

            var act = () => _service.CancelBookingAsync(otherId, booking.BookingId, CancellationToken.None);

            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("You can only cancel your own bookings");
            booking.Status.Should().Be("Confirmed");
        }

        [Fact]
        public async Task CancelBookingAsync_Throws_WhenAlreadyCancelled()
        {
            var userId = Guid.NewGuid();
            var booking = new Booking
            {
                BookingId = Guid.NewGuid(), UserId = userId, RoomId = Guid.NewGuid(),
                StartTime = DateTime.UtcNow.AddHours(5), EndTime = DateTime.UtcNow.AddHours(6),
                TotalPrice = 5m, Status = "Cancelled",
            };
            _bookingRepoMock.Setup(r => r.GetByIdAsync(booking.BookingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);

            var act = () => _service.CancelBookingAsync(userId, booking.BookingId, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Booking is already cancelled");
        }

        #endregion

        #region GetRoomBookingsAsync

        [Fact]
        public async Task GetRoomBookingsAsync_ReturnsOnlyConfirmed_OrderedAscending()
        {
            var roomId = Guid.NewGuid();
            var date = DateTime.UtcNow.Date;

            var laterConfirmed = new Booking
            {
                BookingId = Guid.NewGuid(), RoomId = roomId, UserId = Guid.NewGuid(),
                StartTime = date.AddHours(15), EndTime = date.AddHours(16),
                TotalPrice = 5m, Status = "Confirmed",
            };
            var earlierConfirmed = new Booking
            {
                BookingId = Guid.NewGuid(), RoomId = roomId, UserId = Guid.NewGuid(),
                StartTime = date.AddHours(10), EndTime = date.AddHours(11),
                TotalPrice = 5m, Status = "Confirmed",
            };
            var cancelled = new Booking
            {
                BookingId = Guid.NewGuid(), RoomId = roomId, UserId = Guid.NewGuid(),
                StartTime = date.AddHours(12), EndTime = date.AddHours(13),
                TotalPrice = 5m, Status = "Cancelled",
            };

            _bookingRepoMock.Setup(r => r.GetByRoomIdAndDateAsync(roomId, date, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Booking> { laterConfirmed, cancelled, earlierConfirmed });
            _roomApiMock.Setup(c => c.GetRoomByIdAsync(roomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RoomDto { RoomId = roomId, Name = "The Room", PricePerHour = 5m, Status = "Available" });

            var result = await _service.GetRoomBookingsAsync(roomId, date, CancellationToken.None);

            result.Should().HaveCount(2);
            result[0].BookingId.Should().Be(earlierConfirmed.BookingId);
            result[1].BookingId.Should().Be(laterConfirmed.BookingId);
            result.Should().AllSatisfy(b => b.RoomName.Should().Be("The Room"));
        }

        #endregion
    }
}
