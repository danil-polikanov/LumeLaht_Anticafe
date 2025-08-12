using LumeLaht_RoomApi.Core_.Entities.User;

namespace LumeLaht_RoomApi.Core_.Entities
{
    public class Booking
    {
        public Guid BookingId { get; set; }
        public DateTime BookingTime { get; set; }
        public int Duration { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
        public Guid ClientId { get; set; }
        public Client Client { get; set; }
    }
}
