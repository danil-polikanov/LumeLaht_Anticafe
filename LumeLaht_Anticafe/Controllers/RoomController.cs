using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Application.IServices;
using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace LumeLaht_Anticafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IActivityService _activityService;
        private readonly IMapper _mapper;

        public RoomController(IMapper mapper, IRoomService roomService, IActivityService activityService)
        {
            _mapper = mapper;
            _roomService = roomService;
            _activityService = activityService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var rooms = await _roomService.GetAllRoomsAsync(cancellationToken);
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(Guid id, CancellationToken cancellationToken)
        {
            var room = await _roomService.GetRoomByIdAsync(id, cancellationToken);
            if (room == null)
                return NotFound();

            return Ok(room);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateRoomRequest request, CancellationToken cancellationToken)
        {
            var created = await _roomService.CreateRoomAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetRoomById), new { id = created.RoomId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, CreateRoomRequest request, CancellationToken cancellationToken)
        {
            var updated = await _roomService.UpdateRoomAsync(id, request, cancellationToken);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _roomService.DeleteRoomAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPost("filters")]
        public async Task<ActionResult<RoomFilterDto>> GetRoomFilterOptionsAsync(
            [FromBody] RoomFilterDto roomFilterDto, CancellationToken cancellationToken)
        {
            var result = await _roomService.GetFilteredRoomsAsync(roomFilterDto, cancellationToken);
            return Ok(result);
        }

        [HttpGet("activities")]
        public async Task<IActionResult> GetActivities(CancellationToken cancellationToken)
        {
            var result = await _activityService.GetAllActivitiesAsync(cancellationToken);
            return Ok(result);
        }
    }
}
