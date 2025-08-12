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
        public async Task<ActionResult<IEnumerable<RoomResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var rooms = await _roomService.GetAllRoomsAsync(cancellationToken);
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
            if (id ==null)
                return BadRequest("Invalid Room ID");

            if (request == null)
                return BadRequest("Request is null");

            var updated = await _roomService.UpdateRoomAsync(id, request, cancellationToken);
            if (updated == null)
                return NotFound();

            return Ok(updated); // или NoContent() если не возвращаешь ничего
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _roomService.DeleteRoomAsync(id, cancellationToken);
            return NoContent();
        }
        [HttpGet("filters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<RoomFilterDto> GetRoomFilterOptionsAsync(CancellationToken cancellationToken)
        {
            //var rooms = await _context.Rooms
            //    .Include(r => r.Address)
            //    .Include(r => r.RoomActivity)
            //    .ToListAsync();

            //var cities = rooms
            //    .Select(r => r.Address.City)
            //    .Distinct()
            //    .ToList();

            //var activities = rooms
            //    .SelectMany(r => r.RoomActivity.Select(a => a.Name))
            //    .Distinct()
            //    .ToList();

            //var prices = rooms.Select(r => r.PricePerHour);
            //double minPrice = prices.Min();
            //double maxPrice = prices.Max();

            //return new RoomFiltersDto
            //{
            //    Cities = cities,
            //    Activities = activities,
            //    MinPrice = minPrice,
            //    MaxPrice = maxPrice
            //};
            return null;
        }
        
    }
}
