using RoomService.Core.Entities;

namespace RoomService.Application.IServices
{
    public interface IActivityService
    {
        Task<List<Activity>> GetAllActivitiesAsync(CancellationToken cancellationToken);
    }
}
