using System.Security.Claims;
using BookingService.Application.Dto.Booking;
using BookingService.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.API.Controllers
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookingResponse>> Create(
            CreateBookingRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var result = await _bookingService.CreateBookingAsync(userId, request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("my")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BookingResponse>>> GetMyBookings(
            CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var result = await _bookingService.GetUserBookingsAsync(userId, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{bookingId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Cancel(Guid bookingId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            await _bookingService.CancelBookingAsync(userId, bookingId, cancellationToken);
            return NoContent();
        }

        [HttpGet("room/{roomId:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
