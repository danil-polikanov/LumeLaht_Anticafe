using LumeLaht_RoomApi.Core_.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Application.IServices
{
    public interface IActivityService
    {
        Task<List<Activity>> GetAllActivitiesAsync(CancellationToken cancellationToken);
    }
}
