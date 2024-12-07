namespace AradaAPI.Models.DTO.CategoriesDTO
{
    public class GetCategoriesDTO
    {
        public int Id { get; set; }
        public string NameEn { get; set; } = string.Empty;
        public string NameTr { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string CategoryImage { get; set; } = string.Empty;

        public bool visible { get; set; }

        public int ordernumber { get; set; }

        public DateTime createdAt { get; set; }
    }
}
