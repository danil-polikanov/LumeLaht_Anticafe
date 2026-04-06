namespace BookingService.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBookingRepository Bookings { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
