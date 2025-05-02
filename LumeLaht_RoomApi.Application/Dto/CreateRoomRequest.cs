using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Application.Dto
{
    public class CreateRoomRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double PricePerHour { get; set; }
        public bool IsActive { get; set; }
        public int AddressId { get; set; }
    }
}
