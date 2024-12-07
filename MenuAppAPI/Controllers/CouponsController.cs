using AradaAPI.Models;
using AradaAPI.Models.DTO.CouponsDTO;
using AradaAPI.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AradaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponsController : Controller
    {
        private readonly ICouponsRepository couponsRepository;

        public CouponsController(ICouponsRepository couponsRepository)
        {
            this.couponsRepository = couponsRepository;
        }


        [HttpGet]
        public async Task<IActionResult> getCoupons()
        {
            var coupons = await couponsRepository.GetCoupons();

            if (coupons == null)
            {
                return BadRequest("There are no coupons available ");
            }

            return Ok(coupons);
        }


        [HttpPost]
        public async Task<IActionResult> addCoupon([FromBody] AddCouponsRequestDTO request)
        {
            var coupon = new Coupons
            {
                couponCode = request.couponCode,
                afterDiscountPrice = request.afterDiscountPrice,
                isActive = true
            };

            await couponsRepository.AddCoupon(coupon);

            return Ok(coupon);

        }

        [HttpPut]
        public async Task<IActionResult> updateStatus(int id)
        {
            var result = await couponsRepository.updateStatus(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Could not update coupon status");
        }
        
        [HttpDelete]
        public async Task<IActionResult> deleteCouponAsync(int id)
        {
            var result = await couponsRepository.deleteCoupon(id);

            if (result)
            {
                return Ok();

            }else
            {
                return BadRequest("Could not be deleted");
            }
        }

    }
}
