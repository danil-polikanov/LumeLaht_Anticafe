namespace LumeLaht_RoomApi.Application.Dto
{
    public class RoomResponse
    {
        public Guid RoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PricePerHour { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public AddressResponse Address { get; set; }
        public List<ActivityResponse> Activity { get; set; }
        public List<RoomImagesResponseDto> Images { get; set; }
    }
}
