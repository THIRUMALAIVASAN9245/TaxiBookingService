namespace BookingService.API.Test
{
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using BookingService.API.Entities;
    using BookingService.API.Entities.Repository;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class RepositoryTest
    {
        private IRepository repository;

        private Mock<BookingDbContext> movieCruiserDbContext;

        [Fact]
        public void QueryMethodCallRetrunsExeExpectedResult()
        {
            // Arrange
            movieCruiserDbContext = new Mock<BookingDbContext>();
            movieCruiserDbContext.Setup(r => r.Set<WatchList>()).Returns(GetMockWatchList());
            repository = new Repository(movieCruiserDbContext.Object);

            // Act            
            var result = repository.Query<WatchList>();

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(2, result.ToList().Count);
        }

        private static DbSet<WatchList> GetMockWatchList()
        {
            var watchList = new List<WatchList>
            {
                new WatchList { MovieId = 383498, Comments = "Deadpool 2 is review good" },
                new WatchList { MovieId = 299536, Comments = "Avengers: Infinity War is good" }
            };
            DbSet<WatchList> mockWatchLists = GetQueryableMockDbSet(watchList);

            return mockWatchLists;
        }   

        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return dbSet.Object;
        }
    }
}
