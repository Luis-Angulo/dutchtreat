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
                .ForMember(ovm => ovm.OrderItems, confExp => confExp.MapFrom(order => order.Items))
                .ReverseMap()
                ;
            CreateMap<OrderItem, OrderItemViewModel>()
                .ReverseMap()
                ;
        }
    }
}
