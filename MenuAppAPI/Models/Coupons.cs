using System.ComponentModel.DataAnnotations;

namespace AradaAPI.Models
{
    public class Coupons
    {
        [Key]
        public int couponID { get; set; }
        public string couponCode { get; set; } = string.Empty;
        public MenuItem menuItem { get; set; }
        public int afterDiscountPrice { get; set; }
        public bool isActive { get; set; }

        public Coupons()
        {
            isActive = true;
        }
    }



}
