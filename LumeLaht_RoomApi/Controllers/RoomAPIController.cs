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

        public RoomController(IMapper mapper, IRoomService roomService, ILogger<RoomController> logger)
        {
            _mapper = mapper;
            _roomService = roomService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomResponse>>> GetAll()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
                return NotFound();

            var response = _mapper.Map<RoomResponse>(room);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Create(CreateRoomRequest request)
        {
            if (request == null)
                return BadRequest("Request is null");

            var created = await _roomService.CreateRoomAsync(request);
            return CreatedAtAction(nameof(GetRoomById), new { id = created.RoomId }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(int id, CreateRoomRequest request)
        {
            if (id ==null)
                return BadRequest("Invalid Room ID");

            if (request == null)
                return BadRequest("Request is null");

            var updated = await _roomService.UpdateRoomAsync(id, request);
            if (updated == null)
                return NotFound();

            return Ok(updated); // или NoContent() если не возвращаешь ничего
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int id)
        {
            await _roomService.DeleteRoomAsync(id);
            return NoContent();
        }
    }
}
