namespace AradaAPI.Models.DTO.MenuItemsDTO
{
    public class PagedMenuItems
    {
        public int totalRecords { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public List<MenuItemDto> Data { get; set; }
    }
}
