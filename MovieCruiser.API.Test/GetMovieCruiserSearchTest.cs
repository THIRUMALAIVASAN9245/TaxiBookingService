using System.Collections.Generic;
using System.Net;
using System.Net.Http;
namespace BookingService.API.Test
{
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Moq;
    using Moq.Protected;
    using BookingService.API.Controllers;
    using BookingService.API.Model;
    using Newtonsoft.Json;
    using Xunit;

    public class GetMovieCruiserSearchTest
    {
        private GetMovieCruiserSearchRequest getMovieCruiserSearchRequest;

        private GetMovieCruiserSearch getMovieCruiserSearch;

        [Fact]
        public async Task HandleWithValidGetMovieSearchRequestCallGetMovieListAsExpectedResult()
        {
            // Arrange
            var httpClientStatus = HttpStatusCode.OK;
            InitializeFixture(httpClientStatus);

            // Act
            CancellationToken cancellationToken;
            var result = await getMovieCruiserSearch.Handle(getMovieCruiserSearchRequest, cancellationToken);

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(2, result.total_results);
            Assert.Equal(1, result.page);
            Assert.Equal(2, result.results.Count);
        }

        [Fact]
        public async Task HandleWithValidGetMovieSearchRequestCallRetrunsNotFoundResult()
        {
            // Arrange
            var httpClientStatus = HttpStatusCode.NotFound;
            InitializeFixture(httpClientStatus);

            // Act
            CancellationToken cancellationToken;
            var result = await getMovieCruiserSearch.Handle(getMovieCruiserSearchRequest, cancellationToken);

            // Assert  
            Assert.Null(result);
        }

        [Fact]
        public async Task HandleWithNullGetMovieSearchRequestCallRetrunAsExpected()
        {
            // Arrange
            var httpClientStatus = HttpStatusCode.OK;
            InitializeFixture(httpClientStatus);
            getMovieCruiserSearchRequest = null;

            // Act
            CancellationToken cancellationToken;
            var result = await getMovieCruiserSearch.Handle(getMovieCruiserSearchRequest, cancellationToken);

            // Assert  
            Assert.Null(result);
        }

        private void InitializeFixture(HttpStatusCode httpClientStatus)
        {
            var mockHttpClient = MockHttpClient(httpClientStatus);
            getMovieCruiserSearch = new GetMovieCruiserSearch(mockHttpClient);
            getMovieCruiserSearchRequest = new GetMovieCruiserSearchRequest("DeadPool");
        }

        private static HttpClient MockHttpClient(HttpStatusCode httpClientStatus)
        {
            string movieApiResponse = JsonConvert.SerializeObject(MockMovieApiResponse());

            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = httpClientStatus,
                    Content = new StringContent(movieApiResponse, Encoding.UTF8, "application/json")
                }));

            var httpClient = new HttpClient(httpMessageHandler.Object);

            return httpClient;
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
