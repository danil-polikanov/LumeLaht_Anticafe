namespace LumeLaht_RoomApi.Application.Dto.Booking
{
    public class BookingResponse
    {
        public Guid BookingId { get; set; }
        public Guid RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
