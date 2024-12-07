namespace AradaAPI.Models.DTO.CouponsDTO
{
    public class AddCouponsRequestDTO
    {
        public string couponCode { get; set; } = string.Empty;
        public int menuItemID { get; set; }
        public int afterDiscountPrice { get; set; }
    }
}
