using LumeLaht_RoomApi.Core_.Entities.User;

namespace LumeLaht_RoomApi.Core_.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
