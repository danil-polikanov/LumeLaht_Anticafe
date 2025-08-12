using LumeLaht_RoomApi.Core_.Entities;
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
        public string Status { get; set; }
        public Guid AddressId { get; set; }
        public AddressResponse Address { get; set; }
        public List<ActivityResponse> Activities { get; set; }
    }
}
