using BookingService.Core.Entities;

namespace BookingService.Core.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<List<Booking>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<List<Booking>> GetByRoomIdAndDateAsync(Guid roomId, DateTime date, CancellationToken cancellationToken);
    }
}
