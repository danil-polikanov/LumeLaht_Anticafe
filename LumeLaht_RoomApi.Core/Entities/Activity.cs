using System.ComponentModel.DataAnnotations;

namespace LumeLaht_RoomApi.Core_.Entities
{
    public class Activity
    {
        [Required]
        public Guid ActivityId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public List<RoomActivity> RoomActivity { get; set; }
    }
}
