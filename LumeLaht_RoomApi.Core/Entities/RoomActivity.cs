using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LumeLaht_RoomApi.Core_.Entities
{
    public class RoomActivity
    {
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
        public Guid ActivityId { get; set; }
        public Activity Activity { get; set; }
    }

}
