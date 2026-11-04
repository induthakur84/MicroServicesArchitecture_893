using Order.DTO.Request;
using Order.DTO.Response;
using RegisterDependency;

namespace Order.Data.IServices
{
    [RegisterScoped]
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto> GetUserByIdAsync(int id);
        Task<UserResponseDto> CreateUserAsync(UserRequestDto userRequestDto);
        Task<UserResponseDto> UpdateUserAsync(int id, UserRequestDto userRequestDto);
        Task<bool> DeleteUserAsync(int id);
    }
}

