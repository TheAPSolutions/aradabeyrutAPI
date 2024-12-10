using AradaAPI.Data;
using AradaAPI.Models.DTO.MenuItemsDTO;
using AradaAPI.Repositories.Interface;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AradaAPI.Controllers
{
    //https:localhost:xxx/api/menuitems
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemesRepository menuRepository;
        private readonly BlobService blobService;
        private readonly AppDbContext dbContext;

        public MenuItemsController(IMenuItemesRepository menuRepository, BlobService blobService, AppDbContext dbContext)
        {
            this.menuRepository = menuRepository;
            this.blobService = blobService;
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddMenuItem([FromForm] AddItemRequestDto request)
        {
            if (request == null)
            {
                return BadRequest("Item data or image is missing.");
            }

            string imageUrlString;

            // Check if an image was uploaded
            if (request.imageUrl == null || request.imageUrl.Length <= 0)
            {
                // Default image if no image is provided
                imageUrlString = "/uploads/default-image.png"; // Update with your default local file path
            }
            else
            {
                // Upload the image and get the local file URL
                var uploadResult = await blobService.UploadAsync(request.imageUrl);

                if (uploadResult.Error)
                {
                    return StatusCode(500, uploadResult.Status);
                }

                imageUrlString = uploadResult.Blob.Uri;
            }

            // Parse strings to numbers, handling any invalid input
            int.TryParse(request.PriceTr, out int priceTr);
            int.TryParse(request.PriceEn, out int priceEn);
            int.TryParse(request.PriceAr, out int priceAr);
            int.TryParse(request.categoryId, out int categoryId);

            // Create the MenuItem object
            var item = new MenuItem
            {
                NameTr = request.NameTr,
                NameEn = request.NameEn,
                NameAr = request.NameAr,
                DescriptionTr = request.DescriptionTr,
                DescriptionEn = request.DescriptionEn,
                DescriptionAr = request.DescriptionAr,
                PriceTr = priceTr,
                PriceEn = priceEn,
                PriceAr = priceAr,
                imageUrl = imageUrlString,
                categoryId = categoryId,
                CreatedAt = DateTime.UtcNow,
                isVisible = true,
                ItemOrder = 1,
            };

            // Save the item to the repository
            await menuRepository.AddMenuItem(item);

            return Ok(item);
        }


        [HttpPost("withoutImage")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddMenuItemWithoutImage([FromBody] AddItemRequestDto request)
        {
            if (request == null)
            {
                return BadRequest("Category data or image is missing.");
            }
            string imageUrlString;
            if (request.imageUrl == null || request.imageUrl.Length <= 0)
            {
                imageUrlString = "https://apsolutionsapi.online/images/IMG_1059.PNG";
            }
            else
            {
                var imageUrl = await blobService.UploadAsync(request.imageUrl);
                imageUrlString = imageUrl.Blob.Uri;
            }


            // Parse strings to numbers if needed
            int.TryParse(request.PriceTr, out int priceTr);

            int.TryParse(request.PriceEn, out int priceEn);

            int.TryParse(request.PriceAr, out int priceAr);

            int.TryParse(request.categoryId, out int categoryId);


            var item = new MenuItem
            {
                NameTr = request.NameTr,
                NameEn = request.NameEn,
                NameAr = request.NameAr,
                DescriptionTr = request.DescriptionTr,
                DescriptionEn = request.DescriptionEn,
                DescriptionAr = request.DescriptionAr,
                PriceTr = priceTr,
                PriceEn = priceEn,
                PriceAr = priceAr,
                imageUrl = imageUrlString,
                categoryId = categoryId,
                CreatedAt = DateTime.UtcNow,
                isVisible = true,
                ItemOrder = 1,

            };
            await menuRepository.AddMenuItem(item);

            return Ok(item);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetMenuItem(int id)
        {
            var item = await menuRepository.GetMenuItemById(id);

            return Ok(new MenuItemDto
            {
                Id = item.Id,
                PriceEn = item.PriceEn,
                PriceAr = item.PriceAr,
                PriceTr = item.PriceTr,

                imageUrl = item.imageUrl ?? "",
                NameEn = item.NameEn,
                NameAr = item.NameAr,
                NameTr = item.NameTr,

                DescriptionTr = item.DescriptionTr,
                DescriptionEn = item.DescriptionEn,
                DescriptionAr = item.DescriptionAr,

                ItemOrder = item.ItemOrder,

                categoryName = item.Category.NameEn ?? ""

            });

        }

        [HttpGet]
        public async Task<IActionResult> GetAllMenuItems(int pageNumber = 1, int pageSize = 10)
        {
            var totalRecords = await menuRepository.CountAsync();
            var menuItems = await menuRepository.GetPagedMenuItems(pageNumber, pageSize);
            var response = menuItems.Select(item => new MenuItemDto
            {
                Id = item.Id,
                NameTr = item.NameTr,
                NameEn = item.NameEn,
                NameAr = item.NameAr,
                DescriptionTr = item.DescriptionTr,
                DescriptionEn = item.DescriptionEn,
                DescriptionAr = item.DescriptionAr,
                PriceTr = item.PriceTr,
                PriceEn = item.PriceEn,
                PriceAr = item.PriceAr,
                imageUrl = item.imageUrl,
                categoryId = item.categoryId,
                isVisible = item.isVisible,
                ItemOrder = item.ItemOrder

            }).ToList();



            return Ok(new PagedMenuItems
            {
                totalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                CurrentPage = pageNumber,
                Data = response
            });
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllMenuItemsUnPaged()
        {
            var totalRecords = await menuRepository.GetAllMenuItems();
            var response = totalRecords.Select(item => new MenuItemDto
            {
                Id = item.Id,
                NameTr = item.NameTr,
                NameEn = item.NameEn,
                NameAr = item.NameAr,
                DescriptionTr = item.DescriptionTr,
                DescriptionEn = item.DescriptionEn,
                DescriptionAr = item.DescriptionAr,
                PriceTr = item.PriceTr,
                PriceEn = item.PriceEn,
                PriceAr = item.PriceAr,
                imageUrl = item.imageUrl,
                categoryId = item.categoryId,
                isVisible = item.isVisible,
                ItemOrder = item.ItemOrder

            }).ToList();



            return Ok(response);
        }


        [HttpGet("byCategory")]
        public async Task<IActionResult> GetMenuItemsByCategory(int id, int pageNumber, int pageSize)
        {
            // Get the total count of items for the specified category
            var totalRecords = await menuRepository.GetTotalMenuItemsCountByCategory(id);

            // Fetch the paginated menu items for the specified category
            var menuItems = await menuRepository.GetPagedMenuItemsByCategory(pageNumber, pageSize, id);

            // Map the menu items to DTOs
            var response = menuItems.Select(item => new MenuItemDto
            {
                Id = item.Id,
                NameTr = item.NameTr,
                NameEn = item.NameEn,
                NameAr = item.NameAr,
                DescriptionTr = item.DescriptionTr,
                DescriptionEn = item.DescriptionEn,
                DescriptionAr = item.DescriptionAr,
                PriceTr = item.PriceTr,
                PriceEn = item.PriceEn,
                PriceAr = item.PriceAr,
                imageUrl = item.imageUrl,
                categoryId = item.categoryId,
                isVisible = item.isVisible,
                ItemOrder = item.ItemOrder,
            }).ToList();

            // Return paginated response
            return Ok(new PagedMenuItems
            {
                totalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                CurrentPage = pageNumber,
                Data = response
            });
        }
        [HttpGet("Category")]
        public async Task<IActionResult> GetMenuItemsByCaT(int id)
        {

            // Fetch the paginated menu items for the specified category
            var menuItems = await menuRepository.GetMenuItemsByCategory(id);

            // Map the menu items to DTOs
            var response = menuItems.Select(item => new MenuItemDto
            {
                Id = item.Id,
                NameTr = item.NameTr,
                NameEn = item.NameEn,
                NameAr = item.NameAr,
                DescriptionTr = item.DescriptionTr,
                DescriptionEn = item.DescriptionEn,
                DescriptionAr = item.DescriptionAr,
                PriceTr = item.PriceTr,
                PriceEn = item.PriceEn,
                PriceAr = item.PriceAr,
                imageUrl = item.imageUrl,
                categoryId = item.categoryId,
                isVisible = item.isVisible,
                ItemOrder = item.ItemOrder,
                videoUrlAr = item.Video?.videoUrlAr ?? "",
                videoUrlEn = item.Video?.videoUrlEn ?? "",
                videoUrlTr = item.Video?.videoUrlTr ?? "",
            }).ToList();


            // Return paginated response
            return Ok(response);
        }


        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> EditMenuItem([FromRoute] int id, [FromBody] UpdateMenuItem request)
        {
            //Convert to Dto
            var item = new MenuItem
            {
                Id = id,
                NameTr = request.NameTr,
                NameEn = request.NameEn,
                NameAr = request.NameAr,
                DescriptionTr = request.DescriptionTr,
                DescriptionEn = request.DescriptionEn,
                DescriptionAr = request.DescriptionAr,
                PriceTr = int.Parse(request.PriceTr),
                PriceEn = int.Parse(request.PriceEn),
                PriceAr = int.Parse(request.PriceAr),
                categoryId = int.Parse(request.categoryId!),
                isVisible = request.isVisible,
            };

            item = await menuRepository.UpdateMenuItem(item);
            if (item == null)
            {
                return NotFound();
            }
            return Ok();
        }


        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteMenuItem([FromRoute] int id)
        {
            var item = await menuRepository.DeleteMenuItem(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPut("image")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditItemImage([FromForm] UpdateItemImage request)
        {
            if (request == null || request.image == null)
            {
                return BadRequest("Invalid request or missing image.");
            }

            // Attempt to parse the ID from the request
            if (!int.TryParse(request.Id, out int id))
            {
                return BadRequest("Invalid item ID.");
            }

            // Retrieve the item from the repository
            var item = await menuRepository.getItem(id);
            if (item == null)
            {
                return NotFound($"Item with ID {id} not found.");
            }

            // Upload the new image to the server
            var uploadResult = await blobService.UploadAsync(request.image);

            if (uploadResult.Error)
            {
                return StatusCode(500, uploadResult.Status); // Handle upload error
            }

            // Update the item's image URL
            item.imageUrl = uploadResult.Blob.Uri; // Local file URL (e.g., /uploads/filename.jpg)

            // Save changes to the database
            await dbContext.SaveChangesAsync();

            return Ok(item);
        }


        [HttpGet("search")]
        public async Task<IActionResult> SearchMenuItems([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query string cannot be null or empty.");
            }

            var menuItems = await menuRepository.GetMatchingMenuItemsAsync(query);

            if (menuItems == null || menuItems.Count == 0)
            {
                return NotFound("No matching Menu Items found.");
            }
            var filredMenuItems = menuItems.Select(item => new MenuItemDto
            {
                Id = item.Id,
                NameTr = item.NameTr,
                NameEn = item.NameEn,
                NameAr = item.NameAr,
                DescriptionTr = item.DescriptionTr,
                DescriptionEn = item.DescriptionEn,
                DescriptionAr = item.DescriptionAr,
                PriceTr = item.PriceTr,
                PriceEn = item.PriceEn,
                PriceAr = item.PriceAr,
                ItemOrder = item.ItemOrder,
                isVisible = item.isVisible,
                categoryId = item.categoryId,
                imageUrl = item.imageUrl,

            }).ToList();

            return Ok(filredMenuItems);
        }

    }
}
