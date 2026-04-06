namespace BookingService.Application.Dto
{
    public class RoomDto
    {
        public Guid RoomId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PricePerHour { get; set; }
    }
}
