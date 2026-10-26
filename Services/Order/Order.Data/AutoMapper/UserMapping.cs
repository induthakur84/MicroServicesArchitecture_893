using AutoMapper;
using Order.Domains;
using Order.DTO.Request;
using Order.DTO.Response;

namespace Order.Data.AutoMapper
{
    public class UserMapping:Profile
    {

        public UserMapping()
        {
            CreateMap<User, UserResponseDto>();
            CreateMap<UserRequestDto, User> ();
        }
    }
}
