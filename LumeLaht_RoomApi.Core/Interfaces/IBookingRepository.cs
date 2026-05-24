using LumeLaht_RoomApi.Core_.Entities;

namespace LumeLaht_RoomApi.Core_.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<List<Booking>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<List<Booking>> GetByRoomIdAndDateAsync(Guid roomId, DateTime date, CancellationToken cancellationToken);
        Task<bool> HasConflictAsync(Guid roomId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken);
    }
}
