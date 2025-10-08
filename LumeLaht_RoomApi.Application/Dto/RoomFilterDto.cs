using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Application.Dto
{
    public class RoomFilterDto
    {
        public RoomOptionDTO roomOptionDTO {  get; set; }
        public SortOptions SortOptions { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 6;
        // Конвертируем в универсальный FilterOptions
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
                    { "ActivityIds", roomOptionDTO.ActivityIds }
                }
            };
        }
    }
}
