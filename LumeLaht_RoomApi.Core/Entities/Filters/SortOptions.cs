using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Core_.Entities.Filters
{
    public class SortOptions
    {
        public string? Field { get; set; } = "name";

        public string? Direction { get; set; } = "asc";
    }
}
