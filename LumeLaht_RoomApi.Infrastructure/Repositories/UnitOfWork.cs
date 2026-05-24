using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Entities.User;
using LumeLaht_RoomApi.Core_.Interfaces;
using LumeLaht_RoomApi.Infrastructure.Data;

namespace LumeLaht_RoomApi.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IRoomRepository Rooms { get; }
        public IRepository<Address> Addresses { get; }
        public IRepository<Activity> Activities { get; }
        public IRepository<RoomImage> RoomImages { get; }
        public IUserRepository Users { get; }
        public IBookingRepository Bookings { get; }

        public UnitOfWork(AppDbContext context,
                          IRoomRepository rooms,
                          IRepository<Address> addresses,
                          IRepository<Activity> activities,
                          IRepository<RoomImage> roomImages,
                          IUserRepository users,
                          IBookingRepository bookings)
        {
            _context = context;
            Rooms = rooms;
            Addresses = addresses;
            Activities = activities;
            RoomImages = roomImages;
            Users = users;
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
