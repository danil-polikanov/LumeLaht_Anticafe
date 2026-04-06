using RoomService.Core.Entities.Filters;

namespace RoomService.Application.Dto
{
    public class RoomFilterDto
    {
        public RoomOptionDTO roomOptionDTO {  get; set; }
        public SortOptions SortOptions { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } =3;

        public FilterOptions ToFilterOptions()
        {
            return new FilterOptions
            {
                Search = roomOptionDTO.Search,
                SortOptions=SortOptions,
                Page = Page,
                PageSize = PageSize,
                Filters = new Dictionary<string, object?>
                {
                    { "Status", roomOptionDTO.Status },
                    { "City" ,roomOptionDTO.City },
                    { "Region",roomOptionDTO.Region},
                    { "MinPrice", roomOptionDTO.MinPrice },
                    { "MaxPrice", roomOptionDTO.MaxPrice },
                    { "MinCapacity", roomOptionDTO.MinCapacity },
                    { "MaxCapacity", roomOptionDTO.MaxCapacity },
                    { "ActivitiesIds", roomOptionDTO.ActivitiesIds }
                }
            };
        }
    }
}
