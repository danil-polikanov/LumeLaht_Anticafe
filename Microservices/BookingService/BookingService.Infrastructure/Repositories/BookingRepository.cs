using BookingService.Core.Entities;
using BookingService.Core.Interfaces;
using BookingService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(BookingDbContext context) : base(context)
        {
        }

        public async Task<List<Booking>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Where(b => b.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Booking>> GetByRoomIdAndDateAsync(
            Guid roomId, DateTime date, CancellationToken cancellationToken)
        {
            var dayStart = date.Date;
            var dayEnd = dayStart.AddDays(1);

            return await _dbSet
                .Where(b => b.RoomId == roomId &&
                            b.StartTime >= dayStart &&
                            b.StartTime < dayEnd)
                .ToListAsync(cancellationToken);
        }
    }
}
