using LumeLaht_RoomApi.Core_.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Core_.Interfaces
{
    public interface IUnitOfWork
    {
        IRoomRepository Rooms { get; }
        IRepository<Address> Address { get;}
        IRepository<Activity> Activities { get;}

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
