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

    public class CreateWatchListTest
    {
        private CreateBookingRequest createWatchListRequest;

        private CreateBooking createWatchList;

        private Mock<IRepository> repository;

        [Fact]
        public async Task HandleWithValidCreateRequestCallSaveAsExpectedResult()
        {
            // Arrange                    
            var watchListModel = new WatchListModel { MovieId = 299536, Comments = "Avengers: Infinity is good" };

            var config = new MapperConfiguration(m => { m.CreateMap<WatchList, WatchListModel>(); });
            var mapper = new Mapper(config);

            repository = new Mock<IRepository>();
            repository.Setup(m => m.Get<WatchList>(It.IsAny<int>()))
              .Returns((WatchList)null);

            createWatchList = new CreateBooking(repository.Object, mapper);
            createWatchListRequest = new CreateBookingRequest(watchListModel);

            // Act
            CancellationToken cancellationToken;
            var result = await createWatchList.Handle(createWatchListRequest, cancellationToken);

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(watchListModel.Comments, result.Comments);
            Assert.Equal(watchListModel.MovieId, result.MovieId);
        }

        [Fact]
        public async Task HandleWithNullCreateRequestCallRetrunNullWithThrowsException()
        {
            // Arrange
            var watchListEntity = new WatchList { MovieId = 299536, Comments = "Avengers: Infinity War is good" };
            var watchListModel = new WatchListModel { MovieId = 299536, Comments = "Avengers: Infinity War" };
            var config = new MapperConfiguration(m => { m.CreateMap<WatchList, WatchListModel>(); });
            var mapper = new Mapper(config);

            repository = new Mock<IRepository>();
            repository.Setup(m => m.Get<WatchList>(It.IsAny<int>()))
                .Returns(watchListEntity);
            createWatchList = new CreateBooking(repository.Object, mapper);
            createWatchListRequest = new CreateBookingRequest(watchListModel);

            // Act
            CancellationToken cancellationToken;
            var result = await createWatchList.Handle(createWatchListRequest, cancellationToken);

            // Assert  
            Assert.Null(result);
        }
    }
}
