using AutoMapper;
using Azure.Core;
using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LumaCove_RoomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly ILogger<RoomController> _logger;
        private readonly IMapper _mapper;

        public RoomController(IMapper mapper,IRoomService roomService, ILogger<RoomController> logger)
        {
            _mapper = mapper;
            _roomService = roomService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetAll()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<RoomResponse>(room);
            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateRoomRequest request)
        {
            var room = _mapper.Map<Room>(request);
            await _roomService.CreateRoomAsync(room);
            var response = _mapper.Map<RoomResponse>(room);
            return CreatedAtAction(nameof(GetRoomById), new { id = room.RoomId }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Room room)
        {
            if (id != room.RoomId)
                return BadRequest();

            await _roomService.UpdateRoomAsync(room);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _roomService.DeleteRoomAsync(id);
            return NoContent();
        }
    }
}
