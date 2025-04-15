using LumaCove_Api.Domain.Entities;

namespace LumaCove_Api.Domain.Dto
{
    public class RoomDto
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double PricePerHour { get; set; }
        public bool IsActive { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public List<RoomActivity> RoomActivity { get; set; }
    }
}
