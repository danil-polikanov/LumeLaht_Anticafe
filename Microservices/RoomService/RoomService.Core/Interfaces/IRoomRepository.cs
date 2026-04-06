using RoomService.Core.Entities;
using RoomService.Core.Entities.Filters;

namespace RoomService.Core.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<PagedResult<Room>> GetFilteredRoomsAsync(
            FilterOptions filterOptions,
            CancellationToken cancellationToken);
    }
}
