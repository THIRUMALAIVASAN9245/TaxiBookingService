namespace BookingService.API.Test
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using BookingService.API.Controllers;
    using BookingService.API.Model;
    using Xunit;

    public class MovieCruiserControllerTest
    {
        private Mock<IMediator> mediatR;

        private BookingController controller;

        [Fact]
        public async Task GetCallsMediatRWithExpectedResult()
        {
            // Arrange                        
            mediatR = new Mock<IMediator>();
            mediatR.Setup(m => m.Send(It.IsAny<GetBookinggRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetMovieApiResponse()));
            controller = new BookingController(mediatR.Object);

            // Act
            var result = await controller.Get() as OkObjectResult;

            // Assert            
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var movieCruiser = result.Value as MovieApiResponse;
            Assert.NotNull(movieCruiser);
            Assert.Equal(2, movieCruiser.total_results);
            Assert.Equal(1, movieCruiser.page);
            Assert.Equal(2, movieCruiser.results.Count);
        }

        [Fact]
        public async Task GetCallsMediatRWithExpectedNotFoundResult()
        {
            // Arrange
            mediatR = new Mock<IMediator>();
            mediatR.Setup(m => m.Send(It.IsAny<GetBookinggRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<MovieApiResponse>(null));
            controller = new BookingController(mediatR.Object);

            // Act
            var result = await controller.Get() as NotFoundObjectResult;

            // Assert
            mediatR.Verify(m => m.Send(It.IsAny<GetBookinggRequest>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.NotNull(result);
            Assert.Equal("No Movies found", result.Value);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetByIdCallsMediatRWithExpectedResult()
        {
            // Arrange            
            mediatR = new Mock<IMediator>();
            mediatR.Setup(m => m.Send(It.IsAny<GetMovieCruiserByIdRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(MockMovieByIdResponse()));
            controller = new BookingController(mediatR.Object);

            // Act
            var result = await controller.Get(123) as OkObjectResult;

            // Assert            
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var movieCruiser = result.Value as MovieByIdResponse;
            Assert.NotNull(movieCruiser);
            Assert.Equal(2, movieCruiser.RecommendationsMovies.results.Count);
            Assert.Equal("Avengers: Infinity War", movieCruiser.MovieByIdModel.Title);
            Assert.Equal("Avengers: Infinity War movie was good", movieCruiser.MovieByIdModel.Comments);
        }

        [Fact]
        public async Task GetByIdCallsMediatRWithExpectedNotFoundResult()
        {
            // Arrange            
            mediatR = new Mock<IMediator>();
            mediatR.Setup(m => m.Send(It.IsAny<GetMovieCruiserByIdRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<MovieByIdResponse>(null));
            controller = new BookingController(mediatR.Object);

            // Act
            var result = await controller.Get(123) as NotFoundObjectResult;

            // Assert            
            mediatR.Verify(m => m.Send(It.IsAny<GetMovieCruiserByIdRequest>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.NotNull(result);
            Assert.Equal("No movie found for given movie id", result.Value);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task PostCallsMediatRWithExpectedNotFoundResult()
        {
            // Arrange            
            mediatR = new Mock<IMediator>();
            mediatR.Setup(m => m.Send(It.IsAny<GetMovieCruiserSearchRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<MovieApiResponse>(null));
            controller = new BookingController(mediatR.Object);

            // Act
            var result = await controller.SearchMovie("Deadpool") as NotFoundObjectResult;

            // Assert            
            mediatR.Verify(m => m.Send(It.IsAny<GetMovieCruiserSearchRequest>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.NotNull(result);
            Assert.Equal("No movie found for given search text", result.Value);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task PostCallsMediatRWithExpectedResult()
        {
            // Arrange            
            mediatR = new Mock<IMediator>();
            mediatR.Setup(m => m.Send(It.IsAny<GetMovieCruiserSearchRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetMovieApiResponse()));
            controller = new BookingController(mediatR.Object);

            // Act
            var result = await controller.SearchMovie("Deadpool") as OkObjectResult;

            // Assert            
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var movieCruiser = result.Value as MovieApiResponse;
            Assert.NotNull(movieCruiser);
            Assert.Equal(2, movieCruiser.total_results);
            Assert.Equal(1, movieCruiser.page);
            Assert.Equal(2, movieCruiser.results.Count);
        }

        private static MovieApiResponse GetMovieApiResponse()
        {
            var movieCruiserModel = new List<WatchListModel>
            {
                new WatchListModel
                {
                    Title = "Deadpool 2",
                    ReleaseDate = "2018-05-15"
                },
                new WatchListModel
                {
                    Title = "Avengers: Infinity War",
                    ReleaseDate = "2018-05-15"
                }
            };

            var movieApiResponse = new MovieApiResponse
            {
                page = 1,
                total_pages = 1,
                total_results = 2,
                results = movieCruiserModel
            };

            return movieApiResponse;
        }

        private static MovieByIdResponse MockMovieByIdResponse()
        {
            var movieApiResponse = new MovieByIdResponse
            {
                MovieByIdModel = MockWatchListModel(),
                RecommendationsMovies = MockMovieApiResponse()
            };

            return movieApiResponse;
        }

        private static WatchListModel MockWatchListModel()
        {
            return new WatchListModel
            {
                Title = "Avengers: Infinity War",
                Comments = "Avengers: Infinity War movie was good",
                MovieId = 1234
            };
        }

        private static MovieApiResponse MockMovieApiResponse()
        {
            var movieCruiserModel = new List<WatchListModel>
            {
                new WatchListModel
                {
                    Title = "Deadpool 2",
                    ReleaseDate = "2018-05-15"
                },
                new WatchListModel
                {
                    Title = "Avengers: Infinity War",
                    ReleaseDate = "2018-05-15"
                }
            };

            var movieApiResponse = new MovieApiResponse
            {
                page = 1,
                total_pages = 1,
                total_results = 2,
                results = movieCruiserModel
            };

            return movieApiResponse;
        }
    }
}