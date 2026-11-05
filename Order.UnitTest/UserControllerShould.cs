using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Order.API.Controllers;
using Order.Data.IServices;
using Order.DTO.Response;

namespace Order.UnitTest
{

    //inside this class we will create unit test for usercontroller
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
        public async Task GiveRequest_WhenGetAllUser_ThenReturnsUser()
        {
            // Arrange
            var users = new List<UserResponseDto>
            {
                new UserResponseDto { Id = 1, Name = "Test", Email = "Test@gmail.com", Description = "Full stack" },
                new UserResponseDto { Id = 2, Name = "Test1", Email = "Test1@gmail.com", Description = "Full stack1" },
                new UserResponseDto { Id = 3, Name = "Test2", Email = "Test2@gmail.com", Description = "Full stack2" }
            };

            _userServiceMock.GetAllUsersAsync().Returns(users);

            // Act
            var result = await _userController.GetAll();

            // Assert
            result.Should().NotBeNull();


            //verify that result will http 200 ok response


            var okResult= Assert.IsType<OkObjectResult>(result.Result);

          //  users.Should().HaveCount(5);

        }

        #endregion
    }
}