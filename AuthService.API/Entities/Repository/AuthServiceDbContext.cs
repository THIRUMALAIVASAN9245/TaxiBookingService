namespace AuthService.API.Entities.Repository
{
    using Microsoft.EntityFrameworkCore;

    public class AuthServiceDbContext : DbContext, IAuthServiceDbContext
    {
        public AuthServiceDbContext() { }

        public AuthServiceDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<UserDetail> UserInfo { get; set; }

        public DbSet<Booking> BookingInfo { get; set; }
    }
}
