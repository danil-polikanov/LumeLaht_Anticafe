using LumeLaht_RoomApi.Application.Dto.Booking;
using LumeLaht_RoomApi.Application.IServices;
using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Interfaces;
using Microsoft.Extensions.Logging;

namespace LumeLaht_RoomApi.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IUnitOfWork unitOfWork, ILogger<BookingService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BookingResponse> CreateBookingAsync(
            Guid userId, CreateBookingRequest request, CancellationToken cancellationToken)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId, cancellationToken);
            if (room == null)
                throw new KeyNotFoundException("Room not found");

            if (room.Status != "Available")
                throw new InvalidOperationException("Room is not available");

            var startTime = new DateTime(
                request.StartTime.Year, request.StartTime.Month, request.StartTime.Day,
                request.StartTime.Hour, 0, 0, DateTimeKind.Utc);
            var endTime = startTime.AddHours(1);

            if (startTime < DateTime.UtcNow)
                throw new InvalidOperationException("Cannot book a slot in the past");

            var allBookings = await _unitOfWork.Bookings.GetAllAsync(cancellationToken);
            var hasConflict = allBookings.Any(b =>
                b.RoomId == request.RoomId &&
                b.Status == "Confirmed" &&
                b.StartTime < endTime &&
                b.EndTime > startTime);

            if (hasConflict)
                throw new InvalidOperationException("This time slot is already booked");

            var booking = new Booking
            {
                BookingId = Guid.NewGuid(),
                RoomId = request.RoomId,
                UserId = userId,
                StartTime = startTime,
                EndTime = endTime,
                TotalPrice = room.PricePerHour,
                Status = "Confirmed"
            };

            await _unitOfWork.Bookings.AddAsync(booking, cancellationToken);

            _logger.LogInformation("Booking created: {BookingId} for room {RoomId}", booking.BookingId, room.RoomId);

            return MapToResponse(booking, room.Name);
        }

        public async Task<List<BookingResponse>> GetUserBookingsAsync(
            Guid userId, CancellationToken cancellationToken)
        {
            var allBookings = await _unitOfWork.Bookings.GetAllAsync(cancellationToken);
            var userBookings = allBookings.Where(b => b.UserId == userId).ToList();

            var rooms = await _unitOfWork.Rooms.GetAllAsync(cancellationToken);
            var roomDict = rooms.ToDictionary(r => r.RoomId, r => r.Name);

            return userBookings.Select(b => MapToResponse(b, roomDict.GetValueOrDefault(b.RoomId, "Unknown")))
                .OrderByDescending(b => b.StartTime)
                .ToList();
        }

        public async Task<bool> CancelBookingAsync(
            Guid userId, Guid bookingId, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId, cancellationToken);
            if (booking == null)
                throw new KeyNotFoundException("Booking not found");

            if (booking.UserId != userId)
                throw new UnauthorizedAccessException("You can only cancel your own bookings");

            if (booking.Status == "Cancelled")
                throw new InvalidOperationException("Booking is already cancelled");

            booking.Status = "Cancelled";
            await _unitOfWork.Bookings.UpdateAsync(booking, cancellationToken);

            _logger.LogInformation("Booking cancelled: {BookingId}", bookingId);
            return true;
        }

        public async Task<List<BookingResponse>> GetRoomBookingsAsync(
            Guid roomId, DateTime date, CancellationToken cancellationToken)
        {
            var allBookings = await _unitOfWork.Bookings.GetAllAsync(cancellationToken);
            var dayStart = date.Date;
            var dayEnd = dayStart.AddDays(1);

            var roomBookings = allBookings
                .Where(b => b.RoomId == roomId &&
                            b.Status == "Confirmed" &&
                            b.StartTime >= dayStart &&
                            b.StartTime < dayEnd)
                .ToList();

            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId, cancellationToken);
            var roomName = room?.Name ?? "Unknown";

            return roomBookings.Select(b => MapToResponse(b, roomName))
                .OrderBy(b => b.StartTime)
                .ToList();
        }

        private static BookingResponse MapToResponse(Booking booking, string roomName)
        {
            return new BookingResponse
            {
                BookingId = booking.BookingId,
                RoomId = booking.RoomId,
                RoomName = roomName,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status,
                CreatedAt = booking.CreatedAt
            };
        }
    }
}
