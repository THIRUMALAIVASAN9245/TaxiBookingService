namespace BookingService.API.Controllers
{
    using BookingService.API.Model;
    using MediatR;

    /// <summary>
    /// CreateBookingRequest class
    /// </summary>
    public class CreateBookingRequest : IRequest<Booking>
    {
        public Booking BookingRequest { get; set; }

        ///<Summary>
        /// CreateBookingRequest constructor
        ///</Summary>  
        ///<param name="bookingRequest">bookingRequest</param>
        public CreateBookingRequest(Booking bookingRequest)
        {
            this.BookingRequest = bookingRequest;
        }
    }
}
