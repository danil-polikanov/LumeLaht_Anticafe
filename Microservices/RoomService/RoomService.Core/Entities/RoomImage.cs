using System.ComponentModel.DataAnnotations;

namespace RoomService.Core.Entities
{
    public class RoomImage
    {
        [Key]
        public Guid ImageId { get; set; }

        [Required]
        public string Url { get; set; }

        [MaxLength(256)]
        public string? CloudinaryPublicId { get; set; }

        public bool IsMain { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
    }
}
