using AradaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AradaAPI.Repositories.Interface
{
    public interface ICategories
    {
        Task<Categories> CreateAsync(Categories category);
        Task<IEnumerable<Categories>> GetAllAsync();
        Task<Categories> getCategory(int id);

        Task<bool> DeleteCategory(int id);

        Task<bool> ToggleVisiblity(int id);

        public Task<List<Categories>> GetMatchingCategoriesAsync(string inputString);
        public Task<int> CountAsync();

        public Task<List<Categories>> GetPagedCategories(int pageNumber, int pageSize);



    }
}
