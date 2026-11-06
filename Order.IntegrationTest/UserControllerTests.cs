using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Middleware.CustomExceptions;
using Order.API.Controllers;
using Order.Data;
using Order.Data.AutoMapper;
using Order.Data.IServices;
using Order.Data.Services;
using Order.Domains;
using Order.DTO.Request;
using Order.DTO.Response;
using Xunit;

namespace Order.IntegrationTest
{
    public class UserControllerTests : IDisposable
    {
        private readonly OrderDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserController _sut;

        public UserControllerTests()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new OrderDbContext(options);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserMapping>();
            });
            _mapper = mapperConfig.CreateMapper();

            _userService = new UserService(_context, _mapper);
            _sut = new UserController(_userService);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task Create_WithValidRequest_CreatesUserSuccessfully()
        {
            var request = new UserRequestDto
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Description = "Software Engineer"
            };

            var actionResult = await _sut.Create(request);

            var createdResult = actionResult.Result
                .Should()
                .BeOfType<CreatedAtActionResult>()
                .Subject;

            var result = createdResult.Value
                .Should()
                .BeOfType<UserResponseDto>()
                .Subject;

            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Name.Should().Be("John Doe");
            result.Email.Should().Be("john.doe@example.com");
            result.Description.Should().Be("Software Engineer");
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnsUser()
        {
            var user = new User
            {
                Name = "Jane Smith",
                Email = "jane.smith@example.com",
                Description = "Product Designer"
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var actionResult = await _sut.GetById(user.Id);

            var okResult = actionResult.Result
                .Should()
                .BeOfType<OkObjectResult>()
                .Subject;

            var result = okResult.Value
                .Should()
                .BeOfType<UserResponseDto>()
                .Subject;

            result.Should().NotBeNull();
            result.Id.Should().Be(user.Id);
            result.Name.Should().Be("Jane Smith");
            result.Email.Should().Be("jane.smith@example.com");
            result.Description.Should().Be("Product Designer");
        }

        [Fact]
        public async Task GetAll_WhenUsersExist_ReturnsAllUsers()
        {
            var users = new List<User>
            {
                new()
                {
                    Name = "Alice",
                    Email = "alice@example.com",
                    Description = "DevOps Specialist"
                },
                new()
                {
                    Name = "Bob",
                    Email = "bob@example.com",
                    Description = "Frontend Developer"
                }
            };

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            var actionResult = await _sut.GetAll();

            var okResult = actionResult.Result
                .Should()
                .BeOfType<OkObjectResult>()
                .Subject;

            var result = okResult.Value
                .Should()
                .BeAssignableTo<IEnumerable<UserResponseDto>>()
                .Subject;

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(u => u.Name == "Alice");
            result.Should().Contain(u => u.Name == "Bob");
        }

        [Fact]
        public async Task Update_WithValidRequest_UpdatesUserSuccessfully()
        {
            var user = new User
            {
                Name = "Original User",
                Email = "original@example.com",
                Description = "Junior QA"
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var updateRequest = new UserRequestDto
            {
                Id = user.Id,
                Name = "Updated User Name",
                Email = "updated@example.com",
                Description = "Senior QA Engineer"
            };

            var actionResult = await _sut.Update(user.Id, updateRequest);

            var okResult = actionResult.Result
                .Should()
                .BeOfType<OkObjectResult>()
                .Subject;

            var result = okResult.Value
                .Should()
                .BeOfType<UserResponseDto>()
                .Subject;

            result.Should().NotBeNull();
            result.Id.Should().Be(user.Id);
            result.Name.Should().Be("Updated User Name");
            result.Email.Should().Be("updated@example.com");
            result.Description.Should().Be("Senior QA Engineer");
        }

        [Fact]
        public async Task Delete_WithValidId_DeletesUserSuccessfully()
        {
            var user = new User
            {
                Name = "User To Delete",
                Email = "todelete@example.com",
                Description = "Temporary Account"
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var actionResult = await _sut.Delete(user.Id);

            actionResult.Should().BeOfType<NoContentResult>();

            var deletedUser = await _context.Users.FindAsync(user.Id);
            deletedUser.Should().BeNull();
        }

        [Fact]
        public async Task GetById_WithInvalidId_ThrowsNotFoundException()
        {
            var invalidId = 99999;

            var act = async () => await _sut.GetById(invalidId);

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage($"User with Id {invalidId} not found.");
        }
    }
}

