using System.ComponentModel.DataAnnotations;

namespace RoomService.Core.Entities
{
    public class Address
    {
        [Required]
        public Guid AddressId { get; set; }
        [Required]
        [MaxLength(100)]
        public string AddressName { get; set; }
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        [Required]
        [MaxLength(100)]
        public string Region { get; set; }
        [Required]
        [MaxLength(10)]
        public string PostalCode { get; set; }
        [Required]
        [MaxLength(100)]
        public string Country { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public List<Room> Rooms { get; set; } = new List<Room>();
    }
}
