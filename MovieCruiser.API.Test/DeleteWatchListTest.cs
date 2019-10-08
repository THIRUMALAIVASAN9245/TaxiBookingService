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

    public class DeleteWatchListTest
    {
        private DeleteWatchListRequest deleteWatchListRequest;

        private DeleteWatchList deleteWatchList;

        private Mock<IRepository> repository;

        [Fact]
        public async Task HandleWithValidDeleteRequestCallDeleteAsExpectedResult()
        {
            // Arrange
            var watchListEntity = new WatchList { MovieId = 299536, Comments = "Avengers: Infinity War is good" };

            repository = new Mock<IRepository>();
            repository.Setup(m => m.Get<WatchList>(It.IsAny<int>()))
               .Returns(watchListEntity);

            var config = new MapperConfiguration(m => { m.CreateMap<WatchList, WatchListModel>(); });
            var mapper = new Mapper(config);

            deleteWatchList = new DeleteWatchList(repository.Object, mapper);
            deleteWatchListRequest = new DeleteWatchListRequest(123);

            // Act
            CancellationToken cancellationToken;
            var result = await deleteWatchList.Handle(deleteWatchListRequest, cancellationToken);

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(watchListEntity.Comments, result.Comments);            
            Assert.Equal(watchListEntity.MovieId, result.MovieId);
        }

        [Fact]
        public async Task HandleWithInValidMovieIdDeleteRequestCallDeleteAsExpectedResult()
        {
            // Arrange         
            repository = new Mock<IRepository>();
            repository.Setup(m => m.Get<WatchList>(It.IsAny<int>()))
                .Returns((WatchList)null);

            var config = new MapperConfiguration(m => { m.CreateMap<WatchList, WatchListModel>(); });
            var mapper = new Mapper(config);

            deleteWatchList = new DeleteWatchList(repository.Object, mapper);
            deleteWatchListRequest = new DeleteWatchListRequest(123);

            // Act
            CancellationToken cancellationToken;
            var result = await deleteWatchList.Handle(deleteWatchListRequest, cancellationToken);

            // Assert  
            Assert.Null(result);
        }

        [Fact]
        public async Task HandleWithNullDeleteRequestCallDeleteAsExpectedResult()
        {
            // Arrange         
            repository = new Mock<IRepository>();
            var config = new MapperConfiguration(m => { m.CreateMap<WatchList, WatchListModel>(); });
            var mapper = new Mapper(config);

            deleteWatchList = new DeleteWatchList(repository.Object, mapper);
            deleteWatchListRequest = null;

            // Act
            CancellationToken cancellationToken;
            var result = await deleteWatchList.Handle(deleteWatchListRequest, cancellationToken);

            // Assert  
            Assert.Null(result);
        }
    }
}
