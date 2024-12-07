namespace AradaAPI.Models.DTO.CategoriesDTO
{
    public class AddCategoryDTO
    {
        public string NameEn { get; set; } = string.Empty;
        public string NameTr { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public IFormFile CategoryImage { get; set; }


    }
}
