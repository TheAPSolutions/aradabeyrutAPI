using AradaAPI.Data;
using AradaAPI.Models;
using AradaAPI.Models.DTO.CategoriesDTO;
using AradaAPI.Models.DTO.CategoriesDTO.MenuAppAPI.Models.DTO.CategoriesDTO;
using AradaAPI.Repositories.Interface;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AradaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase // Use ControllerBase instead of Controller for API controllers
    {
        private readonly ICategories _categoriesRepository; // Use the interface
        private readonly BlobService _blobService;
        private readonly AppDbContext dbcontext;

        public CategoryController(ICategories categoriesRepository, BlobService blobService, AppDbContext _dbcontext)
        {
            _categoriesRepository = categoriesRepository;
            _blobService = blobService;
            dbcontext = _dbcontext;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var allCategories = await _categoriesRepository.GetAllAsync();

            if (allCategories == null || !allCategories.Any())
            {
                return Ok(new List<GetCategoriesDTO>());
            }

            var categories = allCategories.Select(category => new GetCategoriesDTO
            {
                Id = category.Id,
                NameAr = category.NameAr,
                NameEn = category.NameEn,
                NameTr = category.NameTr,
                CategoryImage = category.CategoryImage,
                visible = category.IsVisible,
                ordernumber = category.CategoryOrder,

            }).ToList();

            return Ok(categories);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedCategoryAsync(int page, int pageSize)
        {
            var totalRecords = await _categoriesRepository.CountAsync();
            var categoryItems = await _categoriesRepository.GetPagedCategories(page, pageSize);

            var response = categoryItems.Select(category => new GetCategoriesDTO
            {
                Id = category.Id,
                NameAr = category.NameAr,
                NameEn = category.NameEn,
                NameTr = category.NameTr,
                CategoryImage = category.CategoryImage,
                visible = category.IsVisible,
                ordernumber = category.CategoryOrder,
                createdAt = category.CreatedAt,


            }).ToList();

            return Ok(new PagedCategoryItems
            {
                totalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                CurrentPage = page,
                Data = response
            });

        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id)
        {
            var category = await _categoriesRepository.getCategory(id);
            if (category == null)
            {
                return NotFound();
            }

            var response = new GetCategoriesDTO
            {
                NameTr = category.NameTr,
                CategoryImage = category.CategoryImage,
                NameAr = category.NameAr,
                NameEn = category.NameEn,
                Id = category.Id,
                visible = category.IsVisible,
            };

            return Ok(response);

        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategoryAsync([FromForm] AddCategoryDTO category)
        {
            if (category == null || category.CategoryImage == null)
            {
                return BadRequest("Category data or image is missing.");
            }

            // Upload the file to the server and retrieve the URL
            var uploadResult = await _blobService.UploadAsync(category.CategoryImage);

            if (uploadResult.Error)
            {
                return StatusCode(500, uploadResult.Status);
            }

            var length = await _categoriesRepository.GetAllAsync();

            // Create a new Categories object with the uploaded image URL and other data from DTO
            var newCategory = new Categories
            {
                CategoryImage = uploadResult.Blob.Uri ?? "", // Store the local image URL
                CategoryOrder = length.Count() + 1,
                NameAr = category.NameAr,
                NameEn = category.NameEn,
                NameTr = category.NameTr,
                CreatedAt = DateTime.UtcNow,
                IsVisible = true
            };

            await _categoriesRepository.CreateAsync(newCategory);

            return Ok(newCategory);
        }


        [HttpPut("image")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditImageCategory([FromForm] UpdateCategoryImage request)
        {
            if (request == null || request.image == null)
            {
                return BadRequest("Invalid request or missing image.");
            }

            // Upload the new image and retrieve the URL
            var uploadResult = await _blobService.UploadAsync(request.image);

            if (uploadResult.Error)
            {
                return StatusCode(500, uploadResult.Status);
            }

            if (int.TryParse(request.Id, out int id))
            {
                var category = await _categoriesRepository.getCategory(id);
                if (category == null)
                {
                    return NotFound($"Category with ID {id} not found.");
                }

                // Update the category image URL
                category.CategoryImage = uploadResult.Blob.Uri;

                await dbcontext.SaveChangesAsync();

                return Ok(category);
            }

            return BadRequest("Invalid category ID.");
        }



        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditCategory([FromBody] UpdateCategoryRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            // Retrieve the actual category object
            var category = await _categoriesRepository.getCategory(request.Id);

            if (category == null)
            {
                return NotFound($"Category with ID {request.Id} not found.");
            }

            // Update category name based on the type
            if (!string.IsNullOrEmpty(request.Value))
            {
                switch (request.Type.ToLower())
                {
                    case "nameen":
                        category.NameEn = request.Value;
                        break;
                    case "nametr":
                        category.NameTr = request.Value;
                        break;
                    case "namear":
                        category.NameAr = request.Value;
                        break;
                    default:
                        return BadRequest("Invalid category type.");
                }
            }

            // Update category image if a new image is provided
            if (request.Image != null)
            {
                var uploadResult = await _blobService.UploadAsync(request.Image);

                if (uploadResult.Error)
                {
                    return StatusCode(500, uploadResult.Status);
                }

                category.CategoryImage = uploadResult.Blob.Uri; // Local image URL
            }

            // Save changes to the database
            await dbcontext.SaveChangesAsync();

            return Ok(category);
        }


        [HttpDelete]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var isdeleted = await _categoriesRepository.DeleteCategory(id);
            if (isdeleted.Equals(true))
            {
                return Ok();

            }
            return BadRequest();
        }

        [HttpPut("hide")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> ToggleVisibility(int id)
        {
            var executed = await _categoriesRepository.ToggleVisiblity(id);
            if (executed.Equals(true))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }


        [HttpPost("percentage")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> ApplyPercentages([FromBody] ApplyCategoryPercentageDTO request)
        {
            var menuItems = dbcontext.MenuItems.Include(o => o.Category).Where(s => s.Category.Id == request.id).ToList();

            if (menuItems == null || menuItems.Count == 0)
            {
                return BadRequest("There are no menu items under this category");
            }

            foreach (var item in menuItems)
            {
                double factor = request.type.Equals("increase")
                    ? 1 + request.percentage / 100.0
                    : 1 - request.percentage / 100.0;

                if (request.entity.Equals("item"))
                {
                    item.PriceAr = (int)Math.Round(item.PriceAr * factor);
                    item.PriceTr = (int)Math.Round(item.PriceTr * factor);
                    item.PriceEn = (int)Math.Round(item.PriceEn * factor);
                }
            }

            await dbcontext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCategories([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query string cannot be null or empty.");
            }

            var categories = await _categoriesRepository.GetMatchingCategoriesAsync(query);

            if (categories == null || categories.Count == 0)
            {
                return NotFound("No matching categories found.");
            }
            var filredcategories = categories.Select(category => new GetCategoriesDTO
            {
                Id = category.Id,
                NameAr = category.NameAr,
                NameEn = category.NameEn,
                NameTr = category.NameTr,
                CategoryImage = category.CategoryImage,
                visible = category.IsVisible

            }).ToList();

            return Ok(filredcategories);
        }

        [HttpGet("order")]
        public async Task<IActionResult> GetCategoriestoOrder()
        {
            var categories = await _categoriesRepository.GetAllAsync();
            var filtredCategories = categories.Where(c => c.IsVisible).ToList();

            if (filtredCategories.Any())
            {
                var orderedCategories = filtredCategories.Select(s => new OrderCategoriesDTO
                {
                    id = s.Id,
                    categoryName = s.NameEn,
                    orderNumber = s.CategoryOrder
                }).ToList();

                return Ok(orderedCategories);
            }
            else
            {
                return BadRequest("No visible categories found.");
            }
        }

    }
}
