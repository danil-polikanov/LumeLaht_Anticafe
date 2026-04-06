namespace RoomService.Application.Dto
{
    public class RoomImagesResponseDto
    {
        public Guid ImageId { get; set; }
        public string Url { get; set; }
        public string? CloudinaryPublicId { get; set; }
        public bool IsMain { get; set; }
        public Guid RoomId { get; set; }
    }
}
