namespace BookingService.API.Test
{
    using AutoMapper;
    using BookingService.API.Contract;
    using BookingService.API.Entities;
    using BookingService.API.Model;
    using Xunit;

    public class WatchListModelMappingProfileTest
    {
        private MapperConfiguration config;

        [Fact]
        public void ValidateAutoMapperConfiguration()
        {
            // Arrange            
            MapperInitialize();
            config = new MapperConfiguration(s => { s.AddProfile<BookingMappingProfile>(); });

            // Assert      
            Mapper.Configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void WatchListModelMappingProfileShouldMapEntityWatchListToModelWatchListAsExpected()
        {
            // Arrange     
            var entityWatchList = new WatchList { MovieId = 383498, Comments = "Deadpool 2 is review good" };
            config = new MapperConfiguration(s => { s.AddProfile<BookingMappingProfile>(); });

            // Act
            var result = config.CreateMapper().Map<WatchListModel>(entityWatchList);

            // Assert                        
            Assert.Equal(entityWatchList.MovieId, result.MovieId);
            Assert.Equal(entityWatchList.Comments, result.Comments);
        }

        [Fact]
        public void WatchListModelMappingProfileShouldMapModelWatchListToEntityWatchListAsExpected()
        {
            // Arrange     
            var modelWatchList = new WatchListModel { MovieId = 383498, Comments = "Deadpool 2 is review good" };
            config = new MapperConfiguration(s => { s.AddProfile<BookingMappingProfile>(); });

            // Act
            var result = config.CreateMapper().Map<WatchList>(modelWatchList);

            // Assert            
            Assert.Equal(modelWatchList.MovieId, result.MovieId);
            Assert.Equal(modelWatchList.Comments, result.Comments);
        }

        private static void MapperInitialize()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {                
                cfg.CreateMap<WatchList, WatchListModel>()
                    .ForMember(model => model.IsWatchList, option => option.Ignore());
            });           
        }
    }
}
