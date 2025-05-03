using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Core_.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Core_.Interfaces
{
    public interface IRoomService
    {
        Task<List<RoomResponse>> GetAllRoomsAsync();
        Task<RoomResponse> GetRoomByIdAsync(int id);
        Task<RoomResponse> UpdateRoomAsync(int id, CreateRoomRequest request);
        Task<RoomResponse> CreateRoomAsync(CreateRoomRequest request);
        Task<bool> DeleteRoomAsync(int id);
    }
}
