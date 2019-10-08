namespace BookingService.API.Controllers
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using BookingService.API.Entities.Repository;
    using MediatR;
    using AutoMapper.QueryableExtensions;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// GetBookingById class
    /// </summary>
    public class GetBooking : IRequestHandler<GetBookingRequest, List<Model.Booking>>
    {
        private readonly HttpClient httpClient;

        private IRepository repository;

        private IMapper mapper;

        /// <summary>
        /// GetBookingById constructor
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public GetBooking(HttpClient httpClient, IRepository repository, IMapper mapper)
        {
            this.httpClient = httpClient;
            this.repository = repository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Handle Method to get booking rides
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<Model.Booking>> Handle(GetBookingRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return await Task.FromResult<List<Model.Booking>>(null);
            }

            if (request.bookingRequest.Operation == "GetByCustomer")
            {
                var getByCustomer = repository.Query<Entities.Booking>().Where(x => x.CustomerId == request.bookingRequest.Id).ProjectTo<Model.Booking>();

                return await Task.FromResult(getByCustomer.ToList());

            }
            else if (request.bookingRequest.Operation == "GetByEmployee")
            {
                var getByEmployee = repository.Query<Entities.Booking>().Where(x => x.EmployeeId == request.bookingRequest.Id).ProjectTo<Model.Booking>();

                return await Task.FromResult(getByEmployee.ToList());
            }

            var wishlistMovies = repository.Query<Entities.Booking>().ProjectTo<Model.Booking>();

            return await Task.FromResult(wishlistMovies.ToList());
        }
    }
}
