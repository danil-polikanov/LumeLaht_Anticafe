using System.ComponentModel.DataAnnotations;

namespace BookingService.Application.Dto.Booking
{
    public class CreateBookingRequest
    {
        [Required]
        public Guid RoomId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }
    }
}
