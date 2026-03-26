using MapsterMapper;
using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Entities.Filters;
using LumeLaht_RoomApi.Core_.Interfaces;
using System.ComponentModel.DataAnnotations;

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
            var address = await _uow.Addresses.GetByIdAsync(request.AddressId, cancellationToken)
                ?? throw new ValidationException($"Address with ID {request.AddressId} does not exist.");

            if (request.ActivityIds != null && request.ActivityIds.Any())
            {
                foreach (var activityId in request.ActivityIds)
                {
                    var activity = await _uow.Activities.GetByIdAsync(activityId, cancellationToken)
                        ?? throw new ValidationException($"Activity with ID {activityId} does not exist.");
                }
            }

            var room = _mapper.Map<Room>(request);
            room.RoomActivity = request.ActivityIds?.Select(id => new RoomActivity
            {
                ActivityId = id,
                Room = room
            }).ToList();

            await _uow.Rooms.AddAsync(room, cancellationToken);
            return _mapper.Map<RoomResponse>(room);
        }

        public async Task<RoomResponse> UpdateRoomAsync(Guid id, CreateRoomRequest request, CancellationToken cancellationToken)
        {
            var room = _mapper.Map<Room>(request);
            room.RoomId = id;
            room.UpdatedAt = DateTime.UtcNow;
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
