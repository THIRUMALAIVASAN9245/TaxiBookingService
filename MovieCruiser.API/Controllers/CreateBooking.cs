namespace BookingService.API.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using BookingService.API.Model;
    using MediatR;
    using BookingService.API.Entities.Repository;

    /// <summary>
    /// CreateBooking class
    /// </summary>
    public class CreateBooking : IRequestHandler<CreateBookingRequest, Booking>
    {
        private IRepository repository;

        private IMapper mapper;

        /// <summary>
        /// CreateBooking constructor
        /// </summary>
        /// <param name="repository">IRepository</param>
        /// <param name="mapper">IMapper</param>
        public CreateBooking(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Handle method to create new ride
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Booking> Handle(CreateBookingRequest request, CancellationToken cancellationToken)
        {
            var createBooking = mapper.Map<Booking, Entities.Booking>(request.BookingRequest);

            repository.Save(createBooking);

            var createBookingModel = mapper.Map<Entities.Booking, Booking>(createBooking);

            return await Task.FromResult(createBookingModel);
        }
    }
}
