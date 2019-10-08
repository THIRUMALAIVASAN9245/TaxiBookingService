using System.Collections.Generic;
using System.Linq;
namespace BookingService.API.Test
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Moq;
    using BookingService.API.Controllers;
    using BookingService.API.Entities;
    using BookingService.API.Entities.Repository;
    using BookingService.API.Model;
    using Xunit;

    public class GetWatchListTest
    {
        private GetWatchListRequest getWatchListRequest;

        private GetWatchList getWatchList;

        private Mock<IRepository> repository;

        [Fact]
        public async Task HandleWithValidGetWatchListRequestCallGetWatchListRequestAsExpectedResult()
        {
            // Arrange         
            repository = new Mock<IRepository>();
            repository.Setup(m => m.Query<WatchList>())
                .Returns(GetMockWatchList());

            getWatchList = new GetWatchList(repository.Object);
            getWatchListRequest = new GetWatchListRequest();
            MapperInitialize();

            // Act
            CancellationToken cancellationToken;
            var result = await getWatchList.Handle(getWatchListRequest, cancellationToken);

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task HandleWithNullGetWatchListRequestCallRetrunsNotFoundResult()
        {
            // Arrange
            repository = new Mock<IRepository>();
            getWatchList = new GetWatchList(repository.Object);
            getWatchListRequest = null;

            // Act
            CancellationToken cancellationToken;
            var result = await getWatchList.Handle(getWatchListRequest, cancellationToken);

            // Assert  
            Assert.Null(result);
        }

        private static IQueryable<WatchList> GetMockWatchList()
        {
            var watchList = new List<WatchList>
            {
                new WatchList
                {
                    MovieId = 383498,
                    Comments = "Deadpool 2 is review good"
                },
                new WatchList
                {
                    MovieId = 299536,
                    Comments = "Avengers: Infinity War is good"
                }
            };

            return watchList.AsQueryable();
        }

        private static void MapperInitialize()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<WatchList, WatchListModel>();
            });
        }
    }
}