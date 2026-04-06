using Mapster;
using RoomService.Application.Dto;
using RoomService.Core.Entities;

namespace RoomService.Application.Mapping
{
    public class RoomProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateRoomRequest, Room>()
                .Ignore(dest => dest.RoomActivity)
                .Ignore(dest => dest.Images);

            config.NewConfig<Room, RoomResponse>()
                .Map(dest => dest.Activity, src => src.RoomActivity != null
                    ? src.RoomActivity.Select(ra => ra.Activity)
                    : Enumerable.Empty<Activity>());
        }
    }
}
