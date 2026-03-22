using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Core_.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<PagedResult<Room>> GetFilteredRoomsAsync(
            FilterOptions filterOptions,
            CancellationToken cancellationToken);
    }
}
