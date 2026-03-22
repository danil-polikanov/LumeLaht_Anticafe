using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Application.Dto
{
    public class AddressResponse
    {
        public Guid AddressId { get; set; }
        public string AddressName {  get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
    }
}
