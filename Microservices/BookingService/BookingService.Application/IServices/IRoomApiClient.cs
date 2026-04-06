using BookingService.Application.Dto;

namespace BookingService.Application.IServices
{
    public interface IRoomApiClient
    {
        Task<RoomDto?> GetRoomByIdAsync(Guid roomId, CancellationToken ct);
    }
}
