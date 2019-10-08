namespace AuthService.API.Test
{
    using AuthService.API.Controllers;
    using AuthService.API.Models;
    using AuthService.API.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    public class AuthServiceControllerTest
    {
        [Fact]
        public void PostCallsWithUserInfoReturnsExpectedResult()
        {
            //Arrange
            var mockUserService = new Mock<IUserService>();
            var mockTokenGenerator = new Mock<ITokenGenerator>();
            mockUserService.Setup(service => service.Add(It.IsAny<UserModel>())).Returns(1);
            var controller = new AuthServiceController(mockUserService.Object, mockTokenGenerator.Object);
            var user = new UserModel { UserId = "12345", FirstName = "Thirumalai", LastName = "Vasan", Password = "Test@123" };

            //Act
            var result = controller.Post(user);

            //Assert
            var actionResult = Assert.IsType<CreatedResult>(result);
            var value = Assert.IsAssignableFrom<int>(actionResult.Value);
            Assert.Equal(1, value);
            mockUserService.Verify(service => service.Add(It.Is<UserModel>(i => i.UserId == user.UserId)), Times.Once);
        }

        [Fact]
        public void LoginCallsWithUserInfoReturnsExpectedResult()
        {
            //Arrange
            var user = new UserModel { UserId = "12345", Password = "Test@123" };
            var mockUserService = new Mock<IUserService>();
            var mockTokenGenerator = new Mock<ITokenGenerator>();
            mockUserService.Setup(service => service.GetUser(user.UserId, user.Password)).Returns(user);
            mockTokenGenerator.Setup(service => service.GetJwtTokenLoggedinUser(user.UserId)).Returns("Movie_Cruiser_Security_Key");
            var controller = new AuthServiceController(mockUserService.Object, mockTokenGenerator.Object);

            //Act
            var result = controller.Login(user);

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Movie_Cruiser_Security_Key", actionResult.Value);
            Assert.Equal(200, actionResult.StatusCode);
        }
    }
}
