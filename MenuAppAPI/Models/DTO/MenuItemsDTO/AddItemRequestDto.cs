namespace AradaAPI.Models.DTO.MenuItemsDTO
{
    public class AddItemRequestDto
    {
        public string NameTr { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }

        public string DescriptionTr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;

        public string PriceTr { get; set; }
        public string PriceEn { get; set; }
        public string PriceAr { get; set; }

        public IFormFile? imageUrl { get; set; }

        public string? categoryId { get; set; }

    }
}
