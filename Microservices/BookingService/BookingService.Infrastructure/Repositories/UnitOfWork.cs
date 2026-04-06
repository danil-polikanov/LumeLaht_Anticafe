using BookingService.Core.Interfaces;
using BookingService.Infrastructure.Data;

namespace BookingService.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookingDbContext _context;

        public IBookingRepository Bookings { get; }

        public UnitOfWork(BookingDbContext context, IBookingRepository bookings)
        {
            _context = context;
            Bookings = bookings;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _context.SaveChangesAsync(cancellationToken);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
