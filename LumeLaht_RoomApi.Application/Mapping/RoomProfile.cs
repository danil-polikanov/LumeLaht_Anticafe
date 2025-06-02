using AutoMapper;
using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Core_.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LumeLaht_RoomApi.Application.Mapping
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<CreateRoomRequest, Room>()
                .ForMember(dest => dest.RoomActivity, opt => opt.Ignore());
            CreateMap<Room, RoomResponse>()
                .ForMember(dest=>dest.Activity,opt=>opt
                .MapFrom(src=>src.RoomActivity
                .Select(ra=>ra.Activity)));
            CreateMap<Address, AddressResponse>();
            CreateMap<AddressResponse, Address>();
            CreateMap<Activity, ActivityResponse>();
            CreateMap<ActivityResponse, Activity>();
        }
    }

}
