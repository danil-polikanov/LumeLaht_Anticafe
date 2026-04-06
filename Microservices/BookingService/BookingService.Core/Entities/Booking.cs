using System.ComponentModel.DataAnnotations;

namespace BookingService.Core.Entities
{
    public class Booking
    {
        [Key]
        public Guid BookingId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Confirmed";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid RoomId { get; set; }

        public Guid UserId { get; set; }
    }
}
