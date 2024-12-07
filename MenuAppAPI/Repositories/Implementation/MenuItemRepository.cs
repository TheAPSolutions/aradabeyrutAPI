using AradaAPI.Data;
using AradaAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AradaAPI.Repositories.Implementation
{
    public class MenuItemRepository : IMenuItemesRepository
    {
        private readonly AppDbContext dbContext;
        private readonly BlobService blobservice;

        public MenuItemRepository(AppDbContext dbContext, BlobService blobservice)
        {
            this.dbContext = dbContext;
            this.blobservice = blobservice;
        }
        public async Task<MenuItem> AddMenuItem(MenuItem menuItem)
        {

            await dbContext.MenuItems.AddAsync(menuItem);
            await dbContext.SaveChangesAsync();
            return menuItem;
        }

        public async Task<IEnumerable<MenuItem>> GetAllMenuItems()
        {
            return await dbContext.MenuItems.ToListAsync();

        }

        public async Task<MenuItem?> UpdateMenuItem(MenuItem menuItem)
        {

            var existingMenuItem = await dbContext.MenuItems.FirstOrDefaultAsync(x => x.Id == menuItem.Id);
            if (existingMenuItem != null)
            {
                existingMenuItem.NameTr = menuItem.NameTr;
                existingMenuItem.NameEn = menuItem.NameEn;
                existingMenuItem.NameAr = menuItem.NameAr;
                existingMenuItem.DescriptionAr = menuItem.DescriptionAr;
                existingMenuItem.DescriptionEn = menuItem.DescriptionEn;
                existingMenuItem.DescriptionTr = menuItem.DescriptionTr;
                existingMenuItem.PriceTr = menuItem.PriceTr;
                existingMenuItem.PriceAr = menuItem.PriceAr;
                existingMenuItem.PriceEn = menuItem.PriceEn;
                existingMenuItem.isVisible = menuItem.isVisible;
                existingMenuItem.categoryId = menuItem.categoryId;


                await dbContext.SaveChangesAsync();
                return existingMenuItem;

            }
            return null;

        }

        public async Task<MenuItem> DeleteMenuItem(int id)
        {
            var existingItem = await dbContext.MenuItems.FirstOrDefaultAsync(x => x.Id == id);
            if (existingItem is null)
            {
                return null;
            }
            await blobservice.DeleteAsync(existingItem.imageUrl);
            dbContext.Remove(existingItem);
            await dbContext.SaveChangesAsync();
            return existingItem;

        }

        public async Task<List<MenuItem>> GetMatchingMenuItemsAsync(string inputString)
        {
            // Use MySqlParameter for MariaDB
            var inputParam = new MySqlConnector.MySqlParameter("@Name", inputString);

            // Call the stored procedure using the CALL statement
            var matchingMenuItems = await dbContext.MenuItems
                .FromSqlRaw("CALL SearchNames(@Name)", inputParam)
                .ToListAsync();

            return matchingMenuItems;
        }

        public async Task<List<MenuItem>> GetMenuItemsByCategory(int categoryId)
        {
            // Use MySqlParameter for MariaDB
            var inputParam = new MySqlConnector.MySqlParameter("@Id", categoryId);

            // Call the stored procedure using the CALL statement
            var menuItemsByCategory = await dbContext.MenuItems
                .FromSqlRaw("CALL GetMenuItemsById(@Id)", inputParam)
                .ToListAsync();

            return menuItemsByCategory;
        }


        public async Task<List<MenuItem>> GetPagedMenuItemsByCategory(int pageNumber, int pageSize, int id)
        {
            // Await the result of GetMenuItemsByCategory to retrieve the actual list
            var menuItems = await GetMenuItemsByCategory(id);

            // Apply pagination and return the paged results
            return menuItems
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        public async Task<int> GetTotalMenuItemsCountByCategory(int categoryId)
        {
            return await dbContext.MenuItems.CountAsync(item => item.categoryId == categoryId);
        }


        public async Task<int> CountAsync()
        {
            return await dbContext.MenuItems.CountAsync();
        }
        public async Task<List<MenuItem>> GetPagedMenuItems(int pageNumber, int pageSize)
        {
            return await dbContext.MenuItems
                .OrderByDescending(m => m.CreatedAt) // Order by CreatedAt descending
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<MenuItem> getItem(int id)
        {
            var item = await dbContext.MenuItems.FirstOrDefaultAsync(c => c.Id == id);
            if (item == null)
            {
                return null;
            }
            return item;
        }

        public async Task<MenuItem> GetMenuItemById(int id)
        {
            var menuItem = dbContext.MenuItems.Include(o => o.Category).FirstOrDefault(s => s.Id == id);
            return menuItem;
        }
    }
}
