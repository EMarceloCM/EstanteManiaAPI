using AutoMapper;
using EstanteMania.DiscountAPI.DTO_s;
using EstanteMania.DiscountAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EstanteMania.DiscountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICouponRepository _repository;

        public CouponController(IMapper mapper, ICouponRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("{couponCode}")]
        public async Task<ActionResult<CouponDTO>> GetCouponByCode(string couponCode)
        {
            var coupon = await _repository.GetCouponByIdAsync(couponCode);

            if (coupon == null) return NotFound($"Coupon with code: {couponCode} was not found.");

            return _mapper.Map<CouponDTO>(coupon);
        }
    }
}
