
namespace AradaAPI.Repositories.Interface
{
    public interface IMenuItemesRepository
    {
        Task<MenuItem> AddMenuItem(MenuItem menuItem);
        Task<IEnumerable<MenuItem>> GetAllMenuItems();

        Task<MenuItem?> UpdateMenuItem(MenuItem menuItem);
        Task<MenuItem> DeleteMenuItem(int id);

        Task<MenuItem> getItem(int id);


        public Task<List<MenuItem>> GetMatchingMenuItemsAsync(string inputString);
        Task<int> CountAsync();
        Task<List<MenuItem>> GetPagedMenuItems(int pageNumber, int pageSize);

        public Task<List<MenuItem>> GetMenuItemsByCategory(int inputString);
        public Task<List<MenuItem>> GetPagedMenuItemsByCategory(int pageNumber, int pageSize, int id);
        public Task<int> GetTotalMenuItemsCountByCategory(int categoryId);

        public Task<MenuItem> GetMenuItemById(int id);






    }
}
