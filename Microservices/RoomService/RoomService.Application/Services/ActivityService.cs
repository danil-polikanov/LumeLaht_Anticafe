using RoomService.Application.IServices;
using RoomService.Core.Entities;
using RoomService.Core.Interfaces;

namespace RoomService.Application.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IUnitOfWork _uow;

        public ActivityService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<Activity>> GetAllActivitiesAsync(CancellationToken cancellationToken)
        {
            var activities = await _uow.Activities.GetAllAsync(cancellationToken);
            return activities.OrderBy(x => x.Name).ToList();
        }
    }
}
