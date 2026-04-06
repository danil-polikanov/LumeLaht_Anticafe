using System.Security.Claims;
using LumeLaht_RoomApi.Application.Dto.Booking;
using LumeLaht_RoomApi.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumeLaht_Anticafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<ActionResult<BookingResponse>> Create(
            CreateBookingRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var result = await _bookingService.CreateBookingAsync(userId, request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<BookingResponse>>> GetMyBookings(
            CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var result = await _bookingService.GetUserBookingsAsync(userId, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{bookingId:guid}")]
        public async Task<ActionResult> Cancel(Guid bookingId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            await _bookingService.CancelBookingAsync(userId, bookingId, cancellationToken);
            return NoContent();
        }

        [HttpGet("room/{roomId:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<BookingResponse>>> GetRoomBookings(
            Guid roomId, [FromQuery] DateTime date, CancellationToken cancellationToken)
        {
            var result = await _bookingService.GetRoomBookingsAsync(roomId, date, cancellationToken);
            return Ok(result);
        }

        private Guid GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return Guid.Parse(claim!.Value);
        }
    }
}
