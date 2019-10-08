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

            CreateMap<Entities.Booking, Model.Booking>()
                .ForMember(model => model.CustomerDetail, option => option.Ignore())
                .ForMember(model => model.EmployeeDetail, option => option.Ignore());
        }
    }
}
