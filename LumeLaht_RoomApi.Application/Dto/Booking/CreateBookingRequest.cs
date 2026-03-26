using System.ComponentModel.DataAnnotations;

namespace LumeLaht_RoomApi.Application.Dto.Booking
{
    public class CreateBookingRequest
    {
        [Required]
        public Guid RoomId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }
    }
}
