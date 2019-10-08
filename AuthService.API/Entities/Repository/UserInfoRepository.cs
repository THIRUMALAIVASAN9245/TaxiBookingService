namespace AuthService.API.Entities.Repository
{
    using System;
    using System.Linq;

    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly IAuthServiceDbContext authServiceDbContext;

        public UserInfoRepository(IAuthServiceDbContext context)
        {
            authServiceDbContext = context;
        }

        public UserDetail GetById(UserDetail userDetail)
        {
            return authServiceDbContext.UserInfo.FirstOrDefault(x => x.Email == userDetail.Email && x.PhoneNumber == userDetail.PhoneNumber);
        }

        public int Add(UserDetail user)
        {
            authServiceDbContext.UserInfo.Add(user);
            return authServiceDbContext.SaveChanges();
        }

        public UserDetail GetUserByUserIdAndPasssword(string email, string password)
        {
            return authServiceDbContext.UserInfo.FirstOrDefault(user => user.Email == email && user.Password == password);
        }

        public int Update()
        {
            return authServiceDbContext.SaveChanges();
        }
    }
}
