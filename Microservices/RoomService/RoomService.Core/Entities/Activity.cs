using System.ComponentModel.DataAnnotations;

namespace RoomService.Core.Entities
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
        [Required]
        [MaxLength(50)]
        public string Category { get; set; }
        public List<RoomActivity> RoomActivity { get; set; }
    }
}
