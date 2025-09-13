using LumeLaht_RoomApi.Core_.Entities;
namespace LumeLaht_RoomApi.Application.Dto
{
    public class RoomResponse
    {
        public Guid RoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double PricePerHour { get; set; }
        public string Status { get; set; }
        public AddressResponse Address { get; set; }
        public List<ActivityResponse> Activity { get; set; }
        public List<RoomImage> Images { get; set; }

    }
}
