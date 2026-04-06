using BookingService.Application.Dto.Booking;

namespace BookingService.Application.IServices
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateBookingAsync(Guid userId, CreateBookingRequest request, CancellationToken cancellationToken);
        Task<List<BookingResponse>> GetUserBookingsAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> CancelBookingAsync(Guid userId, Guid bookingId, CancellationToken cancellationToken);
        Task<List<BookingResponse>> GetRoomBookingsAsync(Guid roomId, DateTime date, CancellationToken cancellationToken);
    }
}
