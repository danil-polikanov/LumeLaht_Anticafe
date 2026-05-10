using FluentAssertions;
using LumeLaht_RoomApi.Application.Dto.Booking;
using LumeLaht_RoomApi.Application.Services;
using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;
using static LumeLaht_RoomApi.Tests.Helpers.TestDataFactory;

namespace LumeLaht_RoomApi.Tests.Services
{
    public class BookingServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IRoomRepository> _roomRepoMock = new();
        private readonly Mock<IBookingRepository> _bookingRepoMock = new();
        private readonly BookingService _service;

        public BookingServiceTests()
        {
            _uowMock.Setup(u => u.Rooms).Returns(_roomRepoMock.Object);
            _uowMock.Setup(u => u.Bookings).Returns(_bookingRepoMock.Object);
            _service = new BookingService(_uowMock.Object, NullLogger<BookingService>.Instance);
        }

        // Always create the booking slot 2 hours into the future so the
        // "cannot book a slot in the past" guard never fires accidentally.
        private static DateTime FutureSlot() =>
            DateTime.UtcNow.AddHours(2).Date.AddHours(DateTime.UtcNow.Hour + 2);

        #region CreateBookingAsync

        [Fact]
        public async Task CreateBookingAsync_ReturnsResponse_WhenSlotIsFree()
        {
            var room = CreateRoom(name: "Cozy Corner", price: 5m, status: "Available");
            var userId = Guid.NewGuid();
            var request = new CreateBookingRequest { RoomId = room.RoomId, StartTime = FutureSlot() };

            _roomRepoMock.Setup(r => r.GetByIdAsync(room.RoomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(room);
            _bookingRepoMock.Setup(r => r.HasConflictAsync(room.RoomId,
                    It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var result = await _service.CreateBookingAsync(userId, request, CancellationToken.None);

            result.Should().NotBeNull();
            result.RoomId.Should().Be(room.RoomId);
            result.RoomName.Should().Be("Cozy Corner");
            result.TotalPrice.Should().Be(5m);
            result.Status.Should().Be("Confirmed");
            result.EndTime.Should().Be(result.StartTime.AddHours(1));
            _bookingRepoMock.Verify(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateBookingAsync_Throws_WhenRoomNotFound()
        {
            var request = new CreateBookingRequest { RoomId = Guid.NewGuid(), StartTime = FutureSlot() };
            _roomRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Room?)null);

            var act = () => _service.CreateBookingAsync(Guid.NewGuid(), request, CancellationToken.None);

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Room not found");
        }

        [Fact]
        public async Task CreateBookingAsync_Throws_WhenRoomUnavailable()
        {
            var room = CreateRoom(status: "Maintenance");
            var request = new CreateBookingRequest { RoomId = room.RoomId, StartTime = FutureSlot() };
            _roomRepoMock.Setup(r => r.GetByIdAsync(room.RoomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(room);

            var act = () => _service.CreateBookingAsync(Guid.NewGuid(), request, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Room is not available");
            _bookingRepoMock.Verify(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateBookingAsync_Throws_WhenStartInPast()
        {
            var room = CreateRoom(status: "Available");
            var request = new CreateBookingRequest
            {
                RoomId = room.RoomId,
                StartTime = DateTime.UtcNow.AddHours(-2),
            };
            _roomRepoMock.Setup(r => r.GetByIdAsync(room.RoomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(room);

            var act = () => _service.CreateBookingAsync(Guid.NewGuid(), request, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Cannot book a slot in the past");
        }

        [Fact]
        public async Task CreateBookingAsync_Throws_WhenSlotConflicts()
        {
            var room = CreateRoom(status: "Available");
            var request = new CreateBookingRequest { RoomId = room.RoomId, StartTime = FutureSlot() };
            _roomRepoMock.Setup(r => r.GetByIdAsync(room.RoomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(room);
            _bookingRepoMock.Setup(r => r.HasConflictAsync(room.RoomId,
                    It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

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
            var older = new Booking
            {
                BookingId = Guid.NewGuid(), RoomId = Guid.NewGuid(), UserId = userId,
                StartTime = DateTime.UtcNow.AddHours(2), EndTime = DateTime.UtcNow.AddHours(3),
                TotalPrice = 5m, Status = "Confirmed",
            };
            var newer = new Booking
            {
                BookingId = Guid.NewGuid(), RoomId = Guid.NewGuid(), UserId = userId,
                StartTime = DateTime.UtcNow.AddHours(10), EndTime = DateTime.UtcNow.AddHours(11),
                TotalPrice = 10m, Status = "Confirmed",
            };
            _bookingRepoMock.Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Booking> { older, newer });

            var result = await _service.GetUserBookingsAsync(userId, CancellationToken.None);

            result.Should().HaveCount(2);
            result[0].BookingId.Should().Be(newer.BookingId);
            result[1].BookingId.Should().Be(older.BookingId);
            // Without an Include() the navigation property is null and the service
            // falls back to "Unknown" — defensive check, not a feature.
            result.Should().AllSatisfy(b => b.RoomName.Should().Be("Unknown"));
        }

        #endregion

        #region CancelBookingAsync

        [Fact]
        public async Task CancelBookingAsync_ReturnsTrue_WhenOwnerCancelsConfirmedBooking()
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
            var otherUserId = Guid.NewGuid();
            var booking = new Booking
            {
                BookingId = Guid.NewGuid(), UserId = ownerId, RoomId = Guid.NewGuid(),
                StartTime = DateTime.UtcNow.AddHours(5), EndTime = DateTime.UtcNow.AddHours(6),
                TotalPrice = 5m, Status = "Confirmed",
            };
            _bookingRepoMock.Setup(r => r.GetByIdAsync(booking.BookingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);

            var act = () => _service.CancelBookingAsync(otherUserId, booking.BookingId, CancellationToken.None);

            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("You can only cancel your own bookings");
            booking.Status.Should().Be("Confirmed");
            _bookingRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()),
                Times.Never);
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
        public async Task GetRoomBookingsAsync_ReturnsOnlyConfirmed_OrderedByStartTimeAscending()
        {
            var roomId = Guid.NewGuid();
            var date = DateTime.UtcNow.Date;
            var room = CreateRoom(name: "Cozy Corner");
            room.RoomId = roomId;

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
            var cancelledShouldBeFiltered = new Booking
            {
                BookingId = Guid.NewGuid(), RoomId = roomId, UserId = Guid.NewGuid(),
                StartTime = date.AddHours(12), EndTime = date.AddHours(13),
                TotalPrice = 5m, Status = "Cancelled",
            };

            _bookingRepoMock.Setup(r => r.GetByRoomIdAndDateAsync(roomId, date, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Booking> { laterConfirmed, cancelledShouldBeFiltered, earlierConfirmed });
            _roomRepoMock.Setup(r => r.GetByIdAsync(roomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(room);

            var result = await _service.GetRoomBookingsAsync(roomId, date, CancellationToken.None);

            result.Should().HaveCount(2);
            result[0].BookingId.Should().Be(earlierConfirmed.BookingId);
            result[1].BookingId.Should().Be(laterConfirmed.BookingId);
            result.Should().AllSatisfy(b => b.RoomName.Should().Be("Cozy Corner"));
        }

        #endregion
    }
}
