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
        [Range(0,9999999)]
        public decimal PricePerHour { get; set; }
        public string Status { get; set; }
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
        public List<RoomActivity> RoomActivity { get; set; }
        public List<RoomImage> Images { get; set; }
    }
}
