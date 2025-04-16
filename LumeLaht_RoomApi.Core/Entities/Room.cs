using System.ComponentModel.DataAnnotations;

namespace LumeLaht_RoomApi.Core_.Entities
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double PricePerHour { get; set; }
        public bool IsActive { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public List<RoomActivity> RoomActivity { get; set; }
    }
}
