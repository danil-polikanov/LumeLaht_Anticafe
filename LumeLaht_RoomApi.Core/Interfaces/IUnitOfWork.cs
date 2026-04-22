using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Entities.User;

namespace LumeLaht_RoomApi.Core_.Interfaces
{
    public interface IUnitOfWork
    {
        IRoomRepository Rooms { get; }
        IRepository<Address> Addresses { get; }
        IRepository<Activity> Activities { get; }
        IRepository<RoomImage> RoomImages { get; }
        IUserRepository Users { get; }
        IBookingRepository Bookings { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void Dispose();
    }
}
