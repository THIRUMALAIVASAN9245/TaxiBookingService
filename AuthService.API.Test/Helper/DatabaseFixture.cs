namespace AuthService.API.Test.Helper
{
    using System;
    using System.Collections.Generic;
    using AuthService.API.Entities;
    using AuthService.API.Entities.Repository;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseFixture : IDisposable
    {
        private List<UserDetail> UserInfo { get; set; }

        public IAuthServiceDbContext dbContext;

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<AuthServiceDbContext>().UseInMemoryDatabase(databaseName: "UserInfoDB").Options;
            dbContext = new AuthServiceDbContext(options);

            // Insert test user data
            dbContext.UserInfo.Add(new UserDetail { UserId = "Thirumalai123", FirstName = "Thirumalai", LastName = "Vasan", Password = "Test@123" });
            dbContext.UserInfo.Add(new UserDetail { UserId = "Rathish123", FirstName = "Rathish", LastName = "Rakshithan", Password = "Test@123" });
            dbContext.UserInfo.Add(new UserDetail { UserId = "Test123", FirstName = "Test", LastName = "Test", Password = "Test@123" });

            dbContext.SaveChanges();
        }

        public void Dispose()
        {
            UserInfo = null;
            dbContext = null;
        }
    }
}
