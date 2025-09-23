using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Interfaces;
using LumeLaht_RoomApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;

        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Room>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Rooms.Include(ad=>ad.Address).Include(ra=>ra.RoomActivity).ThenInclude(aa=>aa.Activity).ToListAsync(cancellationToken);
        }

        public async Task<Room> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Rooms.Include(ad => ad.Address).Include(ra => ra.RoomActivity).ThenInclude(aa=>aa.Activity).FirstOrDefaultAsync(r => r.RoomId == id, cancellationToken);
        }

        public async Task AddAsync(Room room, CancellationToken cancellationToken)
        {
            await _context.Rooms.AddAsync(room, cancellationToken);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Room room, CancellationToken cancellationToken)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Room room, CancellationToken cancellationToken)
        {
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        public async Task GetFilterRooms(CancellationToken cancellationToken)
        {
            var rooms = await _context.Rooms
                .Select(r => new
                {
                    RoomName = r.Name,
                    CityName = r.Address.City,
                    
                })
                .ToListAsync(cancellationToken);
        }
    }
}
