namespace LumeLaht_RoomApi.Application.Dto
{
    public class CreateRoomRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PricePerHour { get; set; }
        public string Status { get; set; }
        public Guid AddressId { get; set; }
        public List<Guid>? ActivityIds { get; set; }
    }
}
