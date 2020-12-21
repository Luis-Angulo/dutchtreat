using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;

namespace DutchTreat.Data.Mappings
{
    public class DutchMappingProfile: Profile
    {
        public DutchMappingProfile()
        {
            CreateMap<Order, OrderViewModel>()
                // map OrderViewModel.OrderId from Order.Id
                .ForMember(ovm => ovm.OrderId, confExp => confExp.MapFrom(order => order.Id))
                .ReverseMap()
                ;
        }
    }
}
