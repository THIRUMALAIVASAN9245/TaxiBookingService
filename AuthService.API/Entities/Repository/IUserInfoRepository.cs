namespace AuthService.API.Entities.Repository
{
    public interface IUserInfoRepository
    {
        UserDetail GetById(UserDetail userDetail);

        int Add(UserDetail user);

        UserDetail GetUserByUserIdAndPasssword(string email, string password);
       
        int Update();
    }
}
