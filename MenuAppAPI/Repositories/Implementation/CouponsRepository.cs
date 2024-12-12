using AradaAPI.Data;
using AradaAPI.Models;
using AradaAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AradaAPI.Repositories.Implementation
{
    public class CouponsRepository : ICouponsRepository
    {
        private readonly AppDbContext appDbContext;

        public CouponsRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Coupons> AddCoupon(Coupons coupon)
        {
            await appDbContext.Coupons.AddAsync(coupon);
            await appDbContext.SaveChangesAsync();
            return coupon;
        }

        public async Task<bool> deleteCoupon(int id)
        {
            var coupon = appDbContext.Coupons.FirstOrDefault(c => c.couponID == id);
            if (coupon != null)
            {
                appDbContext.Coupons.Remove(coupon);
                await appDbContext.SaveChangesAsync(); // Save the changes
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Coupons>> GetCoupons()
        {
            var coupons =  await appDbContext.Coupons.Include(c => c.menuItem).ToListAsync();
            if (coupons.Any() && coupons != null)
            {
                return coupons;
            }
            return null;
        }

        public async Task<bool> updateStatus(int id)
        {
          var coupon = await appDbContext.Coupons.FirstOrDefaultAsync(c => c.couponID == id);
          if(coupon != null)
            {
                coupon.isActive = !coupon.isActive;
                return true;
            }
            return false;
        }

        public async Task<Coupons?> GetCouponByMenuItemId(int menuItemId)
        {
            return await appDbContext.Coupons
                .Include(c => c.menuItem)
                .FirstOrDefaultAsync(c => c.menuItem.Id == menuItemId);
        }
    }
}
