using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _repository;
        public RoomService(IRoomRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Room>> GetAllRoomsAsync() => await _repository.GetAllAsync();

        public async Task<Room> GetRoomByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task CreateRoomAsync(Room room) => await _repository.AddAsync(room);

        public async Task UpdateRoomAsync(Room room) => await _repository.UpdateAsync(room);

        public async Task DeleteRoomAsync(int id) => await _repository.DeleteAsync(id);
    }

}
