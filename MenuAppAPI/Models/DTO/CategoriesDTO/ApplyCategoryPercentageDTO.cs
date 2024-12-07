namespace AradaAPI.Models.DTO.CategoriesDTO
{
    public class ApplyCategoryPercentageDTO
    {
        public int id { get; set; }
        public string type { get; set; } = string.Empty;

        public int percentage { get; set; }

        public string entity { get; set; } = string.Empty;
    }
}
