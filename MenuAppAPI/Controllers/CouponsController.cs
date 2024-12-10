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
        private readonly IMenuItemesRepository menuItemesRepository;

        public CouponsController(ICouponsRepository couponsRepository, IMenuItemesRepository menuItemesRepository)
        {
            this.couponsRepository = couponsRepository;
            this.menuItemesRepository = menuItemesRepository;
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
            var item = await menuItemesRepository.GetMenuItemById(request.menuItemID);
            var coupon = new Coupons
            {
                couponCode = request.couponCode,
                afterDiscountPrice = request.afterDiscountPrice,
                isActive = true,
                menuItem = item
            };

            await couponsRepository.AddCoupon(coupon);

            return Ok();

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
