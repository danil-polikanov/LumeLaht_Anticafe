namespace RoomService.Application.Dto
{
    public class RoomOptionDTO
    {
        public string? Search { get; set; }
        public string? Status { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinCapacity { get; set; }
        public int? MaxCapacity { get; set; }
        public List<Guid>? ActivitiesIds { get; set; }
    }
}
