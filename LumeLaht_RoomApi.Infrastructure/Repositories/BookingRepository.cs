using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Interfaces;
using LumeLaht_RoomApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LumeLaht_RoomApi.Infrastructure.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(AppDbContext context) : base(context) { }

        public async Task<List<Booking>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
            => await _dbSet
                .AsNoTracking()
                .Where(b => b.UserId == userId)
                .Include(b => b.Room)
                .ToListAsync(cancellationToken);

        public async Task<List<Booking>> GetByRoomIdAndDateAsync(Guid roomId, DateTime date, CancellationToken cancellationToken)
        {
            var dayStart = date.Date;
            var dayEnd = dayStart.AddDays(1);
            return await _dbSet
                .AsNoTracking()
                .Where(b => b.RoomId == roomId && b.StartTime >= dayStart && b.StartTime < dayEnd)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasConflictAsync(Guid roomId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken)
            => await _dbSet
                .AsNoTracking()
                .AnyAsync(b =>
                    b.RoomId == roomId &&
                    b.Status == "Confirmed" &&
                    b.StartTime < endTime &&
                    b.EndTime > startTime,
                    cancellationToken);
    }
}
