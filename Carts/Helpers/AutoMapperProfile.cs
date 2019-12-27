using AutoMapper;
using Carts.Data.POCO;
using Carts.Models;

namespace Carts.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.
            CreateMap<OrderTb, OrderViewModel>().ReverseMap();
            CreateMap<OrderItemTb, OrderItemViewModel>().ReverseMap();
            CreateMap<UsersTb, UserViewModel>().ReverseMap();
            CreateMap<ProductTb, ProductViewModel>().ReverseMap();
            CreateMap<InvoiceTb, InvoiceViewModel>().ReverseMap();

        }
    }
}
