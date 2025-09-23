using LumeLaht_RoomApi.Application.IServices;
using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Application.Services
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
            var activities=await _uow.Activities.GetAllAsync(cancellationToken);
            return activities.OrderBy(x => x.Name).ToList();
        }
    }

}
