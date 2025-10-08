using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Core_.Interfaces
{
    public interface IRoomService
    {
        Task<List<RoomResponse>> GetAllRoomsAsync(CancellationToken cancellationToken);
        Task<RoomResponse> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<RoomResponse> UpdateRoomAsync(Guid id, CreateRoomRequest request, CancellationToken cancellationToken);
        Task<RoomResponse> CreateRoomAsync(CreateRoomRequest request, CancellationToken cancellationToken);
        Task<bool> DeleteRoomAsync(Guid id, CancellationToken cancellationToken);
        Task<PagedResult<RoomResponse>> GetFilteredRoomsAsync(RoomFilterDto filter, CancellationToken cancellationToken);

    }
}
