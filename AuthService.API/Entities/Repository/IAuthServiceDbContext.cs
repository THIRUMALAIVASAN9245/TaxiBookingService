namespace AuthService.API.Entities.Repository
{
    using Microsoft.EntityFrameworkCore;    

    public interface IAuthServiceDbContext
    {
        DbSet<UserDetail> UserInfo { get; set; }

        int SaveChanges();
    }
}