namespace BookingService.API.Controllers
{
    using BookingService.API.Model;
    using MediatR;
    using System.Collections.Generic;

    public class GetBookingRequest : IRequest<List<Model.Booking>>
    {
        public BookingRequest bookingRequest { get; set; }

        ///<Summary>
        /// GetBookingByIdRequest constructor
        ///</Summary>  
        ///<param name="bookingRequest">bookingRequest</param>
        public GetBookingRequest(BookingRequest bookingRequest)
        {
            this.bookingRequest = bookingRequest;
        }
    }
}
