using System.ComponentModel.DataAnnotations;

namespace LumaCove_Api.Domain.Entities
{
    public class Activity
    {
        [Required]
        public int ActivityId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public List<RoomActivity> RoomActivity { get; set; }
    }                      
}
