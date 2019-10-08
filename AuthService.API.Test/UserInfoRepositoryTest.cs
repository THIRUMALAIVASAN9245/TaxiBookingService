namespace AuthService.API.Test
{
    using AuthService.API.Entities;
    using AuthService.API.Entities.Repository;
    using AuthService.API.Test.Helper;
    using Xunit;

    public class UserInfoRepositoryTest : IClassFixture<DatabaseFixture>
    {
        private readonly IUserInfoRepository userInfoRepository;

        public UserInfoRepositoryTest(DatabaseFixture databaseFixture)
        {
            this.userInfoRepository = new UserInfoRepository(databaseFixture.dbContext);
        }

        [Fact]
        public void GetByIdAndPassswordWithUserIdAndPasswordReturnsExpectedResult()
        {
            //Act
            var actual = userInfoRepository.GetUserByUserIdAndPasssword("Thirumalai123", "Test@123");

            // Assert            
            Assert.IsAssignableFrom<UserDetail>(actual);
            Assert.NotNull(actual);
            Assert.Equal("Thirumalai123", actual.UserId);
        }

        [Fact]
        public void GetByIdWithUserIdReturnsExpectedResult()
        {
            //Act
            var actual = userInfoRepository.GetById("Test123");

            // Assert
            Assert.IsAssignableFrom<UserDetail>(actual);
            Assert.NotNull(actual);
            Assert.Equal("Test123", actual.UserId);
        }

        [Fact]
        public void AddWithUserInfoReturnsExpectedResult()
        {
            //Arrange    
            var wishList = new UserDetail { UserId = "Vasan123", FirstName = "Vasan", LastName = "Thirumalai", Password = "Test@123" };

            //Act
            var actual = userInfoRepository.Add(wishList);

            // Assert
            Assert.Equal(1, actual);
        }

        [Fact]
        public void UpdateWithUserInfoReturnsExpectedResult()
        {
            //Act
            var wishList = userInfoRepository.GetById("Test123");
            wishList.Password = "Test@12345";
            var actual = userInfoRepository.Update();

            // Assert
            Assert.Equal(1, actual);
        }

        [Fact]
        public void DeleteWithUserInfoReturnsExpectedResult()
        {
            //Act
            var actual = userInfoRepository.Delete("Rathish123");

            // Assert
            Assert.True(actual);
        }
    }
}
