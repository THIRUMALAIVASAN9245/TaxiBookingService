namespace BookingService.API.Controllers
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using BookingService.API.Entities.Repository;
    using MediatR;
    using System.Linq;
    using System.Collections.Generic;
    using BookingService.API.Entities;
    using BookingService.API.Model;

    /// <summary>
    /// GetBookingById class
    /// </summary>
    public class GetBooking : IRequestHandler<GetBookingRequest, List<Model.BookingResponse>>
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
        public async Task<List<Model.BookingResponse>> Handle(GetBookingRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return await Task.FromResult<List<Model.BookingResponse>>(null);
            }

            IQueryable<Model.BookingResponse> queryGetBookings = BookingQuery();

            if (request.bookingRequest.Operation == "GetByCustomer")
            {
                var getByCustomer = queryGetBookings.Where(a => a.CustomerDetails.Id == request.bookingRequest.Id);
                return await Task.FromResult(getByCustomer.ToList());
            }
            else if (request.bookingRequest.Operation == "GetByEmployee")
            {
                var getByEmployee = queryGetBookings.Where(a => a.EmployeeDetails.Id == request.bookingRequest.Id);
                return await Task.FromResult(getByEmployee.ToList());
            }

            return await Task.FromResult(queryGetBookings.ToList());
        }

        private IQueryable<Model.BookingResponse> BookingQuery()
        {
            return repository.Query<Entities.Booking>()
                .GroupJoin(repository.Query<UserDetail>(), cus => cus.CustomerId, od => od.Id, (booking, customer) => new BookingResponse
                {
                    BookingDetails = mapper.Map<Entities.Booking, Model.Booking>(booking),
                    CustomerDetails = mapper.Map<UserDetail, Model.UserModel>(customer.FirstOrDefault()),
                }).GroupJoin(repository.Query<UserDetail>(), o => o.BookingDetails.EmployeeId, od => od.Id, (booking, employeeDetails) => this.getData(booking, employeeDetails));
        }

        private BookingResponse getData(BookingResponse booking, IEnumerable<UserDetail> employeeDetails) => new Model.BookingResponse
        {
            BookingDetails = booking.BookingDetails,
            CustomerDetails = booking.CustomerDetails,
            EmployeeDetails = mapper.Map<UserDetail, Model.UserModel>(employeeDetails.FirstOrDefault())
        };
    }
}