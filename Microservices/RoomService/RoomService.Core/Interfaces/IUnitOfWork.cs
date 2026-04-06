using RoomService.Core.Entities;

namespace RoomService.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IRoomRepository Rooms { get; }
        IRepository<Address> Addresses { get; }
        IRepository<Activity> Activities { get; }
        IRepository<RoomImage> RoomImages { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void Dispose();
    }
}
