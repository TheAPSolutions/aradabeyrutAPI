namespace AradaAPI.Models.DTO.MenuItemsDTO
{
    public class UpdateMenuItem
    {
        public string NameTr { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }

        public string DescriptionTr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }

        public string PriceTr { get; set; }
        public string PriceEn { get; set; }
        public string PriceAr { get; set; }

        public string? categoryId { get; set; }

        public bool isVisible { get; set; }


    }
}
