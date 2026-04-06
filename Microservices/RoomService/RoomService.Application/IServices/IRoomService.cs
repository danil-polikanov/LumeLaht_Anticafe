using RoomService.Application.Dto;
using RoomService.Core.Entities.Filters;

namespace RoomService.Application.IServices
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
