using AradaAPI.Models;
using AradaAPI.Models.DTO.CouponsDTO;
using AradaAPI.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> addCoupon([FromBody] AddCouponsRequestDTO request)
        {
            // Check if a coupon for the menuItem exists and is active
            var existingCoupon = await couponsRepository.GetCouponByMenuItemId(request.menuItemID);

            if (existingCoupon != null && existingCoupon.isActive)
            {
                // Return a response indicating that an active coupon already exists
                return BadRequest();
            }

            // Fetch the menu item by ID
            var item = await menuItemesRepository.GetMenuItemById(request.menuItemID);
            if (item == null)
            {
                return BadRequest();
            }

            // Create a new coupon
            var coupon = new Coupons
            {
                couponCode = request.couponCode,
                afterDiscountPrice = request.afterDiscountPrice,
                isActive = true,
                menuItem = item
            };

            // Add the new coupon to the repository
            await couponsRepository.AddCoupon(coupon);

            return Ok();
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
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

        [HttpGet("CheckMenuItemDiscount")]
        public async Task<IActionResult> CheckMenuItemDiscount(int menuItemId)
        {
            // Fetch the coupon associated with the menu item
            var coupon = await couponsRepository.GetCouponByMenuItemId(menuItemId);

            if (coupon != null && coupon.isActive)
            {
                // Return the afterDiscountPrice if the coupon is active
                return Ok(coupon.afterDiscountPrice);
            }

            // Return false if the coupon does not exist or is not active
            return Ok(false);
        }

    }
}
