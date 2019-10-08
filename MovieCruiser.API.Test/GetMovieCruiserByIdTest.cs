namespace BookingService.API.Test
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Moq;
    using Moq.Protected;
    using BookingService.API.Controllers;
    using BookingService.API.Entities;
    using BookingService.API.Entities.Repository;
    using BookingService.API.Model;
    using Newtonsoft.Json;
    using Xunit;

    public class GetMovieCruiserByIdTest
    {
        private GetMovieCruiserByIdRequest getMovieCruiserByIdRequest;

        private GetMovieCruiserById getMovieCruiserById;

        private Mock<IRepository> repository;

        [Fact]
        public async Task HandleWithValidCreateRequestCallSaveAsExpectedResult()
        {
            // Arrange          
            var watchListEntity = new WatchList { MovieId = 299536, Comments = "Avengers: Infinity War is good" };

            var config = new MapperConfiguration(m => { m.CreateMap<WatchList, WatchListModel>(); });
            var mapper = new Mapper(config);

            repository = new Mock<IRepository>();
            repository.Setup(m => m.Get<WatchList>(It.IsAny<int>()))
              .Returns(watchListEntity);

            var httpClientStatus = HttpStatusCode.OK;
            var mockHttpClient = MockHttpClient(httpClientStatus);

            getMovieCruiserById = new GetMovieCruiserById(mockHttpClient, repository.Object, mapper);
            getMovieCruiserByIdRequest = new GetMovieCruiserByIdRequest(123);

            // Act
            CancellationToken cancellationToken;
            var result = await getMovieCruiserById.Handle(getMovieCruiserByIdRequest, cancellationToken);

            // Assert  
            Assert.NotNull(result);
            Assert.Equal("Avengers: Infinity War", result.MovieByIdModel.Title);
            Assert.Equal("Avengers: Infinity War is good", result.MovieByIdModel.Comments);
            Assert.Equal(2, result.RecommendationsMovies.results.Count);
        }

        [Fact]
        public async Task HandleWithNullCreateRequestCallSaveAsExpectedResult()
        {
            // Arrange                     
            var config = new MapperConfiguration(m => { m.CreateMap<WatchList, WatchListModel>(); });
            var mapper = new Mapper(config);
            repository = new Mock<IRepository>();
            var httpClientStatus = HttpStatusCode.OK;
            var mockHttpClient = MockHttpClient(httpClientStatus);

            getMovieCruiserById = new GetMovieCruiserById(mockHttpClient, repository.Object, mapper);
            getMovieCruiserByIdRequest = null;

            // Act
            CancellationToken cancellationToken;
            var result = await getMovieCruiserById.Handle(getMovieCruiserByIdRequest, cancellationToken);

            // Assert  
            Assert.Null(result);
        }

        private static HttpClient MockHttpClient(HttpStatusCode httpClientStatus)
        {
            string movieApiResponse = JsonConvert.SerializeObject(MockMovieApiResponse());
            string movieByIdModel = JsonConvert.SerializeObject(MockMovieByIdModel());

            var movieByIdModelUrl = "https://api.themoviedb.org/3/movie/123?api_key=2c03b1e2161e459f63486649d2916c20&language=en-US";
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.Is<HttpRequestMessage>(i => i.RequestUri.ToString().Contains(movieByIdModelUrl)), ItExpr.IsAny<CancellationToken>())
            .Returns(Task.FromResult(new HttpResponseMessage
            {
                StatusCode = httpClientStatus,
                Content = new StringContent(movieByIdModel, Encoding.UTF8, "application/json")
            }));

            var movieApiResponseUrl = "https://api.themoviedb.org/3/movie/123/recommendations?api_key=2c03b1e2161e459f63486649d2916c20&language=en-US&page=1";
            httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(i => i.RequestUri.ToString().Contains(movieApiResponseUrl)),
            ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage
            {
                StatusCode = httpClientStatus,
                Content = new StringContent(movieApiResponse, Encoding.UTF8, "application/json")
            }));

            var httpClient = new HttpClient(httpMessageHandler.Object);

            return httpClient;
        }

        private static WatchListModel MockMovieByIdModel()
        {
            return new WatchListModel
            {
                Title = "Avengers: Infinity War",
                ReleaseDate = "2018-05-15"
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
