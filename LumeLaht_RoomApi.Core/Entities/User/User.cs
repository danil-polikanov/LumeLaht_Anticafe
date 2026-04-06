using System.ComponentModel.DataAnnotations;

namespace LumeLaht_RoomApi.Core_.Entities.User
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "Client";

        [MaxLength(20)]
        public string? Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Booking> Bookings { get; set; } = new();
    }
}