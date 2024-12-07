using AradaAPI.Models;

namespace AradaAPI.Repositories.Interface
{
    public interface ICouponsRepository
    {
        Task<Coupons> AddCoupon(Coupons coupon);

        Task<bool> deleteCoupon(int id);

        Task<IEnumerable<Coupons>> GetCoupons();

        Task<bool> updateStatus(int id);

    }
}
