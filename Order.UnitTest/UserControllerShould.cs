using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Middleware.CustomExceptions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Order.API.Controllers;
using Order.Data.IServices;
using Order.DTO.Request;
using Order.DTO.Response;
using Xunit;

namespace Order.UnitTest
{
    public class UserControllerShould
    {
        private readonly IUserService _userServiceMock;
        private readonly UserController _userController;

        public UserControllerShould()
        {
            _userServiceMock = Substitute.For<IUserService>();
            _userController = new UserController(_userServiceMock);
        }

        #region GetAll Tests

        [Fact]
        public async Task GivenRequest_WhenGetAllUsers_ThenReturnsUsers()
        {
            var users = new List<UserResponseDto>
            {
                new UserResponseDto
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john@example.com",
                    Description = "Full Stack Engineer"
                },
                new UserResponseDto
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Email = "jane@example.com",
                    Description = "Cloud Architect"
                }
            };

            _userServiceMock.GetAllUsersAsync().Returns(users);

            var result = await _userController.GetAll();

            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var data = Assert.IsAssignableFrom<IEnumerable<UserResponseDto>>(okResult.Value);
            data.Should().HaveCount(2);
            await _userServiceMock.Received(1).GetAllUsersAsync();
        }

        [Fact]
        public async Task GivenNoUsers_WhenGetAllUsers_ThenReturnsEmptyList()
        {
            var users = new List<UserResponseDto>();

            _userServiceMock
                .GetAllUsersAsync()
                .Returns(users);

            var result = await _userController.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var data = Assert.IsAssignableFrom<IEnumerable<UserResponseDto>>(okResult.Value);
            data.Should().BeEmpty();
            await _userServiceMock.Received(1).GetAllUsersAsync();
        }

        #endregion

        #region GetById Tests

        [Fact]
        public async Task GivenValidId_WhenGetById_ThenReturnsUser()
        {
            var user = new UserResponseDto
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@example.com",
                Description = "Full Stack Engineer"
            };

            _userServiceMock.GetUserByIdAsync(1).Returns(user);

            var result = await _userController.GetById(1);

            result.Should().NotBeNull();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.Value.Should().BeEquivalentTo(user);
            await _userServiceMock.Received(1).GetUserByIdAsync(1);
        }

        [Fact]
        public async Task GivenInvalidId_WhenGetById_ThenThrowsNotFoundException()
        {
            _userServiceMock.GetUserByIdAsync(99)
                .ThrowsAsync(new NotFoundException("User with Id 99 not found."));

            var act = async () => await _userController.GetById(99);

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("User with Id 99 not found.");

            await _userServiceMock.Received(1).GetUserByIdAsync(99);
        }

        #endregion

        #region Create Tests

        [Fact]
        public async Task GivenValidRequest_WhenCreate_ThenReturnsCreatedUser()
        {
            var request = new UserRequestDto
            {
                Name = "John Doe",
                Email = "john@example.com",
                Description = "Full Stack Engineer"
            };

            var response = new UserResponseDto
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@example.com",
                Description = "Full Stack Engineer"
            };

            _userServiceMock.CreateUserAsync(Arg.Any<UserRequestDto>())
                            .Returns(response);

            var result = await _userController.Create(request);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            createdResult.ActionName.Should().Be(nameof(UserController.GetById));
            createdResult.Value.Should().BeEquivalentTo(response);
            await _userServiceMock.Received(1).CreateUserAsync(Arg.Any<UserRequestDto>());
        }

        #endregion

        #region Update Tests

        [Fact]
        public async Task GivenValidRequest_WhenUpdate_ThenReturnsUpdatedUser()
        {
            var request = new UserRequestDto
            {
                Id = 1,
                Name = "Updated John",
                Email = "updated.john@example.com",
                Description = "Lead Engineer"
            };

            var response = new UserResponseDto
            {
                Id = 1,
                Name = "Updated John",
                Email = "updated.john@example.com",
                Description = "Lead Engineer"
            };

            _userServiceMock.UpdateUserAsync(1, Arg.Any<UserRequestDto>())
                            .Returns(response);

            var result = await _userController.Update(1, request);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.Value.Should().BeEquivalentTo(response);
            await _userServiceMock.Received(1).UpdateUserAsync(1, Arg.Any<UserRequestDto>());
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task GivenValidId_WhenDelete_ThenReturnsNoContent()
        {
            _userServiceMock.DeleteUserAsync(1).Returns(true);

            var result = await _userController.Delete(1);

            Assert.IsType<NoContentResult>(result);
            await _userServiceMock.Received(1).DeleteUserAsync(1);
        }

        [Fact]
        public async Task GivenInvalidId_WhenDelete_ThenThrowsNotFoundException()
        {
            _userServiceMock.DeleteUserAsync(99)
                .ThrowsAsync(new NotFoundException("User with Id 99 not found."));

            var act = async () => await _userController.Delete(99);

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("User with Id 99 not found.");

            await _userServiceMock.Received(1).DeleteUserAsync(99);
        }

        #endregion
    }
}
