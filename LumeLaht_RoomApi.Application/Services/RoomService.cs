using MapsterMapper;
using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Entities.Filters;
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
            var address = await _uow.Addresses.GetByIdAsync(request.AddressId, cancellationToken)
                ?? throw new ValidationException($"Address with ID {request.AddressId} does not exist.");

            // 2.Do activities exist
            if (request.Activities != null && request.Activities.Any())
            {
                foreach (var activity in request.Activities)
                {
                    var existing = await _uow.Activities.GetByIdAsync(activity.ActivityId, cancellationToken)
                        ?? throw new ValidationException($"Activity with ID {activity.ActivityId} does not exist.");
                }
            }
            var room = _mapper.Map<Room>(request);
#pragma warning disable CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
            room.RoomActivity = request.Activities?.Select(a => new RoomActivity
            {
                Activity = _mapper.Map<Activity>(a),
                Room = room
            }).ToList();
#pragma warning restore CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.

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
            await _uow.Rooms.DeleteAsync(room.RoomId, cancellationToken);
            return true;
        }
        public async Task<PagedResult<RoomResponse>> GetFilteredRoomsAsync(
           RoomFilterDto filter,
           CancellationToken cancellationToken)
        {
            var filterOptions = filter.ToFilterOptions();
            var result = await _uow.Rooms.GetFilteredRoomsAsync(filterOptions, cancellationToken);

            return new PagedResult<RoomResponse>
            {
                Items = _mapper.Map<List<RoomResponse>>(result.Items),
                pagination = new PaginationOptions
                {
                    CurrentPage = result.pagination.CurrentPage,
                    PageSize = result.pagination.PageSize,
                    TotalItems = result.pagination.TotalItems,

                }
            };
        }
    }

}
