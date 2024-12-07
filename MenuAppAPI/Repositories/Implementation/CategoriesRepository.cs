using AradaAPI.Data;
using AradaAPI.Models;
using AradaAPI.Repositories.Interface;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AradaAPI.Repositories.Implementation
{
    public class CategoriesRepository : ICategories
    {
        private readonly AppDbContext dbContext;
        private readonly BlobService blobService;

        public CategoriesRepository(AppDbContext dbContext, BlobService blobService)
        {
            this.dbContext = dbContext;
            this.blobService = blobService;
        }

        public async Task<Categories> CreateAsync(Categories category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }
        public async Task<bool> DeleteCategory(int id)
        {
            var category = await getCategory(id);

            await blobService.DeleteAsync(category.CategoryImage);
            if (category != null)
            {
                dbContext.Categories.Remove(category);
                await dbContext.SaveChangesAsync(); // Save the changes
                return true;
            }
            return false; // Indicates deletion failed
        }


        public async Task<IEnumerable<Categories>> GetAllAsync()
        {
            return await dbContext.Categories
                .OrderByDescending(c => c.CreatedAt) // Order by CreatedAt descending
                .ToListAsync();
        }

        public async Task<Categories> getCategory(int id)
        {
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return null;
            }
            return category;
        }

        public async Task<bool> ToggleVisiblity(int id)
        {
            var category = await getCategory(id);
            if (category != null)
            {
                category.IsVisible = !category.IsVisible;
            }

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<Categories>> GetMatchingCategoriesAsync(string inputString)
        {
            // Call the stored procedure using raw SQL with the CALL statement
            var inputParam = new MySqlConnector.MySqlParameter("@inputString", inputString);

            var matchingCategories = await dbContext.Categories
                .FromSqlRaw("CALL getMatchingCategories(@inputString)", inputParam)
                .ToListAsync();

            return matchingCategories;
        }


        public async Task<int> CountAsync()
        {
            return await dbContext.Categories.CountAsync();
        }
        public async Task<List<Categories>> GetPagedCategories(int pageNumber, int pageSize)
        {
            return await dbContext.Categories
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
