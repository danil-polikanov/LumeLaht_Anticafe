using RoomService.Core.Entities;
using RoomService.Core.Interfaces;
using RoomService.Infrastructure.Data;

namespace RoomService.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RoomDbContext _context;

        public IRoomRepository Rooms { get; }
        public IRepository<Address> Addresses { get; }
        public IRepository<Activity> Activities { get; }
        public IRepository<RoomImage> RoomImages { get; }

        public UnitOfWork(RoomDbContext context,
                          IRoomRepository rooms,
                          IRepository<Address> addresses,
                          IRepository<Activity> activities,
                          IRepository<RoomImage> roomImages)
        {
            _context = context;
            Rooms = rooms;
            Addresses = addresses;
            Activities = activities;
            RoomImages = roomImages;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _context.SaveChangesAsync(cancellationToken);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
