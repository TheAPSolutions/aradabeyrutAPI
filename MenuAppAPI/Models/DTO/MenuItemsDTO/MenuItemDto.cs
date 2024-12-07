namespace AradaAPI.Models.DTO.MenuItemsDTO
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public string NameTr { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }

        public string DescriptionTr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }

        public int PriceTr { get; set; }
        public int PriceEn { get; set; }
        public int PriceAr { get; set; }


        public int ItemOrder { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool isVisible { get; set; }

        public string? imageUrl { get; set; }

        public int? categoryId { get; set; }

        public string categoryName { get; set; } = string.Empty;

        public string videoUrlTr { get; set; } = string.Empty;
        public string videoUrlEn { get; set; } = string.Empty;
        public string videoUrlAr { get; set; } = string.Empty;


    }
}
