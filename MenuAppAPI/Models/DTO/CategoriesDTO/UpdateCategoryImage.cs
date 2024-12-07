namespace AradaAPI.Models.DTO.CategoriesDTO
{
    public class UpdateCategoryImage
    {
        public string Id { get; set; }
        public IFormFile image { get; set; }
    }
}
