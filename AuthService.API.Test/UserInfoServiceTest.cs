namespace AuthService.API.Test
{
    using AuthService.API.Entities;
    using AuthService.API.Entities.Repository;
    using AuthService.API.Models;
    using AuthService.API.Services;
    using Moq;
    using Xunit;

    public class UserInfoServiceTest
    {
        private readonly UserInfoService userInfoService;

        private readonly Mock<IUserInfoRepository> mockRepository;

        public UserInfoServiceTest()
        {
            mockRepository = new Mock<IUserInfoRepository>();
            userInfoService = new UserInfoService(mockRepository.Object);
        }

        [Fact]
        public void GetUserWithUserInfoReturnsExpectedResult()
        {
            //Arrange            
            var expected = new Entities.UserDetail { UserId = "12345", FirstName = "Thirumalai", LastName = "Vasan", Password = "Test@123" };
            mockRepository.Setup(repo => repo.GetUserByUserIdAndPasssword(It.IsAny<string>(), It.IsAny<string>())).Returns(expected);

            //Act
            var actual = userInfoService.GetUser("12345", "Test@123");

            //Assert            
            Assert.IsAssignableFrom<Models.UserModel>(actual);
            Assert.Equal(expected.UserId, actual.UserId);
        }

        [Fact]
        public void IsUserExistUserWithUserInfoReturnsExpectedResult()
        {
            //Arrange            
            var expected = new Entities.UserDetail { UserId = "12345", FirstName = "Thirumalai", LastName = "Vasan", Password = "Test@123" };
            mockRepository.Setup(repo => repo.GetById(It.IsAny<string>())).Returns(expected);

            //Act
            var actual = userInfoService.IsUserExist("12345");

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public void AddWithUserInfoReturnsExpectedResult()
        {
            //Arrange
            var userInfo = new Models.UserModel { UserId = "12345", FirstName = "Thirumalai", LastName = "Vasan", Password = "Test@123" };
            mockRepository.Setup(repo => repo.Add(It.IsAny<Entities.UserDetail>())).Returns(1);

            //Act
            var actual = userInfoService.Add(userInfo);

            //Assert
            Assert.Equal(1, actual);
            mockRepository.Verify(repo => repo.Add(It.Is<Entities.UserDetail>(i => i.UserId == userInfo.UserId)), Times.Once);
        }
    }
}
