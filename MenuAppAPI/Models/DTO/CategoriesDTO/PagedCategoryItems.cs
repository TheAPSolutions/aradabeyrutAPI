
namespace AradaAPI.Models.DTO.CategoriesDTO
{
    public class PagedCategoryItems
    {
        public int totalRecords { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public List<GetCategoriesDTO> Data { get; set; }
    }
}
