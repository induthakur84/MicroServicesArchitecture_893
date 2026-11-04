using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Middleware.CustomExceptions;
using Order.Data.IServices;
using Order.Domains;
using Order.DTO.Request;
using Order.DTO.Response;

namespace Order.Data.Services
{
    public class UserService : IUserService
    {
        private readonly OrderDbContext _context;
        private readonly IMapper _mapper;

        public UserService(OrderDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException($"User with Id {id} not found.");
            }

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> CreateUserAsync(UserRequestDto userRequestDto)
        {
            if (userRequestDto == null)
            {
                throw new BadRequestException("User data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(userRequestDto.Name))
            {
                throw new BadRequestException("User name is required.");
            }

            var nameExists = await _context.Users.AnyAsync(u => u.Name == userRequestDto.Name);
            if (nameExists)
            {
                throw new BadRequestException($"A user with the name '{userRequestDto.Name}' already exists.");
            }

            var user = _mapper.Map<User>(userRequestDto);
            user.Id = 0; // Ensure new identity

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> UpdateUserAsync(int id, UserRequestDto userRequestDto)
        {
            if (userRequestDto == null)
            {
                throw new BadRequestException("User data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(userRequestDto.Name))
            {
                throw new BadRequestException("User name is required.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException($"User with Id {id} not found.");
            }

            var nameConflict = await _context.Users.AnyAsync(u => u.Name == userRequestDto.Name && u.Id != id);
            if (nameConflict)
            {
                throw new BadRequestException($"A user with the name '{userRequestDto.Name}' already exists.");
            }

            user.Name = userRequestDto.Name;
            user.Email = userRequestDto.Email;
            user.Description = userRequestDto.Description;

            await _context.SaveChangesAsync();

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException($"User with Id {id} not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

