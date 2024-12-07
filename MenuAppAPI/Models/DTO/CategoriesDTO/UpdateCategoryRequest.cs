namespace AradaAPI.Models.DTO.CategoriesDTO
{
    public class UpdateCategoryRequest
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }
    }
}
