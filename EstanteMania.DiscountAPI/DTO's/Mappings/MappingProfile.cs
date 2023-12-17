using AutoMapper;
using EstanteMania.DiscountAPI.Models;

namespace EstanteMania.DiscountAPI.DTO_s.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CouponDTO, Coupon>().ReverseMap();
        }
    }
}
