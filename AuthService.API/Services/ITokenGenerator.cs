namespace AuthService.API.Services
{
    using AuthService.API.Models;

    public interface ITokenGenerator
    {
        string GetJwtTokenLoggedinUser(UserModel userModel);
    }
}
