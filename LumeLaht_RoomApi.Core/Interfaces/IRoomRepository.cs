using LumeLaht_RoomApi.Core_.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Core_.Interfaces
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllAsync(CancellationToken cancellationToken);
        Task<Room> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(Room room,CancellationToken cancellationToken);
        Task UpdateAsync(Room room, CancellationToken cancellationToken);
        Task DeleteAsync(Room room, CancellationToken cancellationToken);
    }
}
