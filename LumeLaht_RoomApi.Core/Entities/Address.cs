using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LumeLaht_RoomApi.Core_.Entities
{
    public class Address
    {
        [Required]
        public int AddressId { get; set; }
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