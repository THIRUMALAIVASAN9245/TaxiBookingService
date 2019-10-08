namespace AuthService.API.Services
{
    using AuthService.API.Models;

    public interface IUserService
    {
        bool IsUserExist(UserModel userModel);

        UserModel GetUser(string email, string password);

        int Add(UserModel userModel);
    }
}
