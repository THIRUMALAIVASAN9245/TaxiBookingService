using System;
using Microsoft.AspNetCore.Http;

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

    public class WatchListControllerTest
    {
        private Mock<IMediator> mediatR;

        private WatchListController controller;

        [Fact]
        public async Task GetCallsMediatRWithExpectedResult()
        {
            // Arrange            
            mediatR = new Mock<IMediator>();
            mediatR.Setup(m => m.Send(It.IsAny<GetWatchListRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetWatchListResponse()));
            controller = new WatchListController(mediatR.Object);

            // Act
            var result = await controller.Get() as OkObjectResult;

            // Assert            
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var watchListModel = result.Value as IList<WatchListModel>;
            Assert.NotNull(watchListModel);
            Assert.Equal(2, watchListModel.Count);
        }

        [Fact]
        public async Task GetCallsMediatRWithExpectedNotFoundResult()
        {
            // Arrange
            mediatR = new Mock<IMediator>();
            mediatR.Setup(m => m.Send(It.IsAny<GetWatchListRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<IList<WatchListModel>>(null));
            controller = new WatchListController(mediatR.Object);

            // Act
            var result = await controller.Get() as NotFoundObjectResult;

            // Assert
            mediatR.Verify(m => m.Send(It.IsAny<GetWatchListRequest>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.NotNull(result);
            Assert.Equal("No watchlist WatchList found", result.Value);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task PostCallsMediatRWithExpectedResult()
        {
            // Arrange
            var watchListModel = new WatchListModel { MovieId = 383498, Comments = "Deadpool 2 is review good" };
            mediatR = new Mock<IMediator>();
            mediatR.Setup(m => m.Send(It.IsAny<CreateBookingRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(watchListModel));
            controller = new WatchListController(mediatR.Object);

            // Act
            var result = await controller.Post(watchListModel) as CreatedResult;

            // Assert            
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            var watchListModels = result.Value as WatchListModel;
            Assert.NotNull(watchListModels);
            Assert.Equal("Deadpool 2 is review good", watchListModels.Comments);
        }

        [Fact]
        public async Task PostCallsMediatRWithExpectedNotFoundResult()
        {
            // Arrange
            var watchListModel = new WatchListModel { MovieId = 383498, Comments = "Deadpool 2 is review good" };
            mediatR = new Mock<IMediator>();
            mediatR.Setup(m => m.Send(It.IsAny<CreateBookingRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<WatchListModel>(null));
            controller = new WatchListController(mediatR.Object);

            // Act
            var result = await controller.Post(watchListModel) as ObjectResult;

            // Assert
            mediatR.Verify(m => m.Send(It.IsAny<CreateBookingRequest>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.NotNull(result);
            Assert.Equal(409, result.StatusCode);
        }

        [Fact]
        public async Task PostCallsWithInValidModelStateExpectedNotFoundResult()
        {
            // Arrange
            var watchListModel = new WatchListModel { MovieId = 383498 };
            mediatR = new Mock<IMediator>();        
            controller = new WatchListController(mediatR.Object);
            controller.ModelState.AddModelError("Comments", "Comments is Required");

            // Act
            var result = await controller.Post(watchListModel) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task PutCallsMediatRWithExpectedResult()
        {
            // Arrange
            var watchListModel = new WatchListModel { MovieId = 383498, Comments = "Deadpool 2 is review good" };
            mediatR = new Mock<IMediator>();
            mediatR.Setup(m => m.Send(It.IsAny<UpdateWatchListRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(watchListModel));
            controller = new WatchListController(mediatR.Object);

            // Act
            var result = await controller.Put(watchListModel) as OkObjectResult;

            // Assert            
            Assert.NotNull(result);
            var watchListModels = result.Value as WatchListModel;
            Assert.NotNull(watchListModels);
            Assert.Equal("Deadpool 2 is review good", watchListModels.Comments);
        }

        [Fact]
        public async Task PutCallsMediatRWithExpectedNotFoundResult()
        {
            // Arrange
            var watchListModel = new WatchListModel { MovieId = 383498, Comments = "Deadpool 2 is review good" };
            mediatR = new Mock<IMediator>();
            mediatR.Setup(m => m.Send(It.IsAny<UpdateWatchListRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<WatchListModel>(null));
            controller = new WatchListController(mediatR.Object);

            // Act
            var result = await controller.Put(watchListModel) as NotFoundObjectResult;

            // Assert
            mediatR.Verify(m => m.Send(It.IsAny<UpdateWatchListRequest>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.NotNull(result);
            Assert.Equal("WatchList Not exists in the DB or error occurred", result.Value);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task PutCallsWithInValidModelStateExpectedNotFoundResult()
        {
            // Arrange
            var watchListModel = new WatchListModel { MovieId = 383498, Comments = "Deadpool 2 is review good" };
            mediatR = new Mock<IMediator>();
            mediatR.Setup(m => m.Send(It.IsAny<UpdateWatchListRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<WatchListModel>(null));
            controller = new WatchListController(mediatR.Object);
            controller.ModelState.AddModelError("Comments", "Comments is Required");

            // Act
            var result = await controller.Put(watchListModel) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task DeleteCallsMediatRWithExpectedResult()
        {
            // Arrange
            var watchListModel = new WatchListModel { MovieId = 383498, Comments = "Deadpool 2 is review good" };
            mediatR = new Mock<IMediator>();
            mediatR.Setup(m => m.Send(It.IsAny<DeleteWatchListRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(watchListModel));
            controller = new WatchListController(mediatR.Object);

            // Act
            var result = await controller.Delete(1) as OkObjectResult;

            // Assert            
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task DeleteCallsMediatRWithExpectedNotFoundResult()
        {
            // Arrange           
            mediatR = new Mock<IMediator>();
            mediatR.Setup(m => m.Send(It.IsAny<DeleteWatchListRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<WatchListModel>(null));
            controller = new WatchListController(mediatR.Object);

            // Act
            var result = await controller.Delete(1) as NotFoundObjectResult;

            // Assert
            mediatR.Verify(m => m.Send(It.IsAny<DeleteWatchListRequest>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.NotNull(result);
            Assert.Equal("WatchList Not exists in the DB or error occurred", result.Value);
            Assert.Equal(404, result.StatusCode);
        }

        private static IList<WatchListModel> GetWatchListResponse()
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

            return movieCruiserModel;
        }
    }
}
