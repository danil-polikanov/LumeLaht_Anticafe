using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Interfaces;
using LumeLaht_RoomApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IRoomRepository Rooms { get; }
        public IRepository<Address> Address { get; }
        public IRepository<Activity> Activities { get; }

        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context,
                          IRoomRepository rooms,
                          IRepository<Address> address,
                          IRepository<Activity> activities)
        {
            _context = context;
            Rooms = rooms;
            Address = address;
            Activities = activities;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _context.SaveChangesAsync(cancellationToken);
    }

}
