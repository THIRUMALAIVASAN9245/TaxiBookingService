namespace AuthService.API.Test
{
    using AuthService.API.Models;
    using AuthService.API.Services;
    using Xunit;

    public class TokenGeneratorTest
    {
        private readonly TokenGenerator tokenGenerator;        

        public TokenGeneratorTest()
        {
            tokenGenerator = new TokenGenerator();
        }

        [Fact]
        public void GetJwtTokenLoggedinUserWithUserIdReturnsExpectedResult()
        {
            //Arrange            
            var userId = "12345";

            //Act
            var actual = tokenGenerator.GetJwtTokenLoggedinUser(userId);

            //Assert                        
            Assert.NotEmpty(actual);
        }
    }
}
