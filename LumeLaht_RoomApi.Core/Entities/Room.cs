using System.ComponentModel.DataAnnotations;

namespace LumeLaht_RoomApi.Core_.Entities
{
    public class Room
    {
        [Key]
        public Guid RoomId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        [Range(0, 9999999)]
        public decimal PricePerHour { get; set; }
        [Required]
        [Range(1, 100)]
        public int Capacity { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
        public List<RoomActivity> RoomActivity { get; set; }
        public List<RoomImage> Images { get; set; }
        public List<Booking> Bookings { get; set; } = new();
    }
}
