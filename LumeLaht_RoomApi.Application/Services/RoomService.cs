using AutoMapper;
using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LumeLaht_RoomApi.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RoomService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<RoomResponse>> GetAllRoomsAsync()
        {
            var rooms = await _uow.Rooms.GetAllAsync();
            return _mapper.Map<List<RoomResponse>>(rooms);
        }

        public async Task<RoomResponse> GetRoomByIdAsync(int id)
        {
            var room = await _uow.Rooms.GetByIdAsync(id);
            return room == null ? null : _mapper.Map<RoomResponse>(room);
        }

        public async Task<RoomResponse> CreateRoomAsync(CreateRoomRequest request)
        {
            // 1. Does address exists
            if (!await _uow.Address.ExistsAsync(a => a.AddressId == request.AddressId))
                throw new ValidationException($"Address with ID {request.AddressId} does not exist.");

            // 2.Do activities exist
            if (request.Activities != null && request.Activities.Any())
            {
                foreach (var activity in request.Activities) {
                    if (!await _uow.Activities.ExistsAsync(a => a.Name == request.Name))
                    {
                        throw new ValidationException($"Activities not found for IDs: {string.Join(", ", request.Name)}");
                    }
                }
            }
            var room = _mapper.Map<Room>(request);
            if (request.Address != null && request.Activities.Any())
            {
                var roomsAddress = _uow.Address.GetByIdAsync(request.AddressId);
                room.RoomActivity = request.Activities.Select(a => new RoomActivity
                {
                    Activity = _mapper.Map<Activity>(a), 
                    Room = room
                }).ToList();
            }

            await _uow.Rooms.AddAsync(room); 
            return _mapper.Map<RoomResponse>(room);
        }
        public async Task<RoomResponse> UpdateRoomAsync(int id,CreateRoomRequest request)
        {
            var room = _mapper.Map<Room>(request);
            room.RoomId = id;
            await _uow.Rooms.UpdateAsync(room);
            return _mapper.Map<RoomResponse>(room);
        }
        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await _uow.Rooms.GetByIdAsync(id);
            if (room == null) return false;
            await _uow.Rooms.DeleteAsync(room);
            return true;
        }
    }

}
