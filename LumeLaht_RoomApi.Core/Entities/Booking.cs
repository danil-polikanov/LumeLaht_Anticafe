using LumeLaht_RoomApi.Core_.Entities.User;

namespace LumeLaht_RoomApi.Core_.Entities
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime BookingTime { get; set; }
        public int Duration { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
