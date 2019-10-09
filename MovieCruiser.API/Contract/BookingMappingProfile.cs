namespace BookingService.API.Contract
{
    using AutoMapper;

    /// <summary>
    /// BookingMappingProfile class
    /// </summary>
    public class BookingMappingProfile : Profile
    {
        /// <summary>
        /// BookingMappingProfile Constructor
        /// </summary>
        public BookingMappingProfile()
        {
            CreateMissingTypeMaps = true;

            CreateMap<Model.Booking, Entities.Booking>();
            CreateMap<Model.UserModel, Entities.UserDetail>();

            CreateMap<Entities.Booking, Model.Booking>();
            CreateMap<Entities.UserDetail, Model.UserModel>();
        }
    }
}
