using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plants.Core.Entities;
using Plants.Core.IServices;

namespace Plants.API.Controllers
{
    [Route("category")]
    [ApiController]
    public class CategoryController : Controller
    {
        private ICategoryService _categoryService;
        private IProductService _productService;

        public CategoryController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var categorys = await _categoryService.GetAll();
            return Ok(categorys);
        }

        [HttpGet("{ID:guid}")]
        public async Task<IActionResult> GetByID(Guid ID)
        {
            var category = await _categoryService.GetByID(ID);
            return Ok(category);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]Category category)
        {
            var categoryReturned = await _categoryService.Add(category);
            return Ok(categoryReturned);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody]Category category)
        {
            var categoryReturned = await _categoryService.Update(category);
            return Ok(categoryReturned);
        }

        [Authorize]
        [HttpDelete("{ID:guid}")]
        public async Task<IActionResult> Delete(Guid ID)
        {
            var category = await _categoryService.GetByID(ID);
            var products = await _productService.GetByCategoryID(category.ID);
            foreach (var product in products)
            {
                if (product.ImagePath != @"Resources\Images\default-tree.png")
                {
                    System.IO.File.Delete(product.ImagePath);
                }
                await _productService.Delete(product.ID);
            }
            if (category != null && category.ImagePath != @"Resources\Images\default-tree.png")
            {
                System.IO.File.Delete(category.ImagePath);
            }

            await _categoryService.Delete(ID);
            return Ok();
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage()
        {
            var file = Request.Form.Files[0];
            var folderName = Path.Combine("Resources", "Images", "Category");
            var path = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(path, fileName);
                var dbPath = Path.Combine(folderName, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    stream.Close();
                }
                

                return Ok(new { dbPath });
            }
            return Ok();
        }
    }
}