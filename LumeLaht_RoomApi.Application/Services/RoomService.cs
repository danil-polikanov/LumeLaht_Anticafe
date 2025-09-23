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
using System.Threading;
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

        public async Task<List<RoomResponse>> GetAllRoomsAsync(CancellationToken cancellationToken)
        {
            var rooms = await _uow.Rooms.GetAllAsync(cancellationToken);
            return _mapper.Map<List<RoomResponse>>(rooms);
        }

        public async Task<RoomResponse> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var room = await _uow.Rooms.GetByIdAsync(id, cancellationToken);
            return room == null ? null : _mapper.Map<RoomResponse>(room);
        }

        public async Task<RoomResponse> CreateRoomAsync(CreateRoomRequest request, CancellationToken cancellationToken)
        {
            // 1. Does address exists
            if (!await _uow.Addresses.ExistsAsync(a => a.AddressId == request.AddressId, cancellationToken))
                throw new ValidationException($"Address with ID {request.AddressId} does not exist.");

            // 2.Do activities exist
            if (request.Activities != null && request.Activities.Any())
            {
                foreach (var activity in request.Activities) {
                    if (!await _uow.Activities.ExistsAsync(a => a.Name == request.Name, cancellationToken))
                    {
                        throw new ValidationException($"Activities not found for IDs: {string.Join(", ", request.Name)}");
                    }
                }
            }
            var room = _mapper.Map<Room>(request);
            if (request.Address != null && request.Activities.Any())
            {
                var roomsAddress = _uow.Addresses.GetByIdAsync(request.AddressId, cancellationToken);
                room.RoomActivity = request.Activities.Select(a => new RoomActivity
                {
                    Activity = _mapper.Map<Activity>(a), 
                    Room = room
                }).ToList();
            }

            await _uow.Rooms.AddAsync(room, cancellationToken); 
            return _mapper.Map<RoomResponse>(room);
        }
        public async Task<RoomResponse> UpdateRoomAsync(Guid id,CreateRoomRequest request,CancellationToken cancellationToken)
        {
            var room = _mapper.Map<Room>(request);
            room.RoomId = id;
            await _uow.Rooms.UpdateAsync(room, cancellationToken);
            return _mapper.Map<RoomResponse>(room);
        }
        public async Task<bool> DeleteRoomAsync(Guid id, CancellationToken cancellationToken)
        {
            var room = await _uow.Rooms.GetByIdAsync(id, cancellationToken);
            if (room == null) return false;
            await _uow.Rooms.DeleteAsync(room, cancellationToken);
            return true;
        }
        public async Task<PagedResult<Room>> GetFilteredRoomsAsync(RoomFilterDto parameters)
        {
            return null;
        }
    }

}
