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

    public class UpdateWatchListTest
    {
        private UpdateWatchListRequest updateWatchListRequest;

        private UpdateWatchList updateWatchList;

        private Mock<IRepository> repository;

        [Fact]
        public async Task HandleWithValidUpdateRequestCallUpdateAsExpectedResult()
        {
            // Arrange          
            var watchListEntity = new WatchList { MovieId = 299536, Comments = "Avengers: Infinity War is good" };
            var watchListModel = new WatchListModel { MovieId = 299536, Comments = "Avengers: Infinity is good" };

            var config = new MapperConfiguration(m => { m.CreateMap<WatchList, WatchListModel>(); });
            var mapper = new Mapper(config);

            repository = new Mock<IRepository>();
            repository.Setup(m => m.Get<WatchList>(It.IsAny<int>()))
              .Returns(watchListEntity);

            updateWatchList = new UpdateWatchList(repository.Object, mapper);
            updateWatchListRequest = new UpdateWatchListRequest(watchListModel);

            // Act
            CancellationToken cancellationToken;
            var result = await updateWatchList.Handle(updateWatchListRequest, cancellationToken);

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(watchListModel.Comments, result.Comments);
            Assert.Equal(watchListModel.MovieId, result.MovieId);
        }

        [Fact]
        public async Task HandleWithInValidUpdateRequestCallUpdateAsExpectedResult()
        {
            // Arrange         
            var watchListModel = new WatchListModel { MovieId = 299536, Comments = "Avengers: Infinity is good" };
            repository = new Mock<IRepository>();
            repository.Setup(m => m.Get<WatchList>(It.IsAny<int>()))
                .Returns((WatchList)null);

            var config = new MapperConfiguration(m => { m.CreateMap<WatchList, WatchListModel>(); });
            var mapper = new Mapper(config);

            updateWatchList = new UpdateWatchList(repository.Object, mapper);
            updateWatchListRequest = new UpdateWatchListRequest(watchListModel);

            // Act
            CancellationToken cancellationToken;
            var result = await updateWatchList.Handle(updateWatchListRequest, cancellationToken);

            // Assert  
            Assert.Null(result);
        }

        [Fact]
        public async Task HandleWithNullUpdateRequestCallUpdateAsExpectedResult()
        {
            // Arrange                    
            repository = new Mock<IRepository>();
            var config = new MapperConfiguration(m => { m.CreateMap<WatchList, WatchListModel>(); });
            var mapper = new Mapper(config);

            updateWatchList = new UpdateWatchList(repository.Object, mapper);
            updateWatchListRequest = null;

            // Act
            CancellationToken cancellationToken;
            var result = await updateWatchList.Handle(updateWatchListRequest, cancellationToken);

            // Assert  
            Assert.Null(result);
        }
    }
}
