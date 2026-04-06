using MapsterMapper;
using RoomService.Application.Dto;
using RoomService.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace RoomService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IActivityService _activityService;
        private readonly ILogger<RoomController> _logger;
        private readonly IMapper _mapper;

        public RoomController(IMapper mapper, IRoomService roomService, IActivityService activityService, ILogger<RoomController> logger)
        {
            _mapper = mapper;
            _roomService = roomService;
            _activityService = activityService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var rooms = await _roomService.GetAllRoomsAsync(cancellationToken);
            if (rooms == null)
            {
                return NotFound();
            }
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoomById(Guid id, CancellationToken cancellationToken)
        {
            var room = await _roomService.GetRoomByIdAsync(id, cancellationToken);
            if (room == null)
                return NotFound();

            var response = _mapper.Map<RoomResponse>(room);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Create(CreateRoomRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                return BadRequest("Request is null");

            var created = await _roomService.CreateRoomAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetRoomById), new { id = created.RoomId }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(Guid id, CreateRoomRequest request, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid Room ID");

            if (request == null)
                return BadRequest("Request is null");

            var updated = await _roomService.UpdateRoomAsync(id, request, cancellationToken);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _roomService.DeleteRoomAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPost("filters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomFilterDto>> GetRoomFilterOptionsAsync([FromBody] RoomFilterDto roomFilterDto, CancellationToken cancellationToken)
        {
            var result = await _roomService.GetFilteredRoomsAsync(roomFilterDto, cancellationToken);
            if (result == null)
            {
                return BadRequest("Invalid Filters");
            }
            return Ok(result);
        }

        [HttpGet("activities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActivities(CancellationToken cancellationToken)
        {
            var result = await _activityService.GetAllActivitiesAsync(cancellationToken);
            return Ok(result);
        }
    }
}
