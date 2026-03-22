using Mapster;
using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Core_.Entities;

namespace LumeLaht_RoomApi.Application.Mapping
{
    public class RoomProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateRoomRequest, Room>()
                .Ignore(dest => dest.RoomActivity);

            config.NewConfig<Room, RoomResponse>()
                .Map(dest => dest.Activity, src => src.RoomActivity.Select(ra => ra.Activity));
        }
    }
}
