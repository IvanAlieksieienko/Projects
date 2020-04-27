using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Motopark.Core.Entities;
using Motopark.Core.IServices;

namespace Motopark.API.Controllers
{
    [Route("category")]
    [ApiController]
    public class CategoryController : Controller
    {
        private ICategoryService<Category> _categoryService;

        public CategoryController(ICategoryService<Category> categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAll();
            return Ok(categories);
        }

        [HttpGet("sub/{id:guid}")]
        public async Task<IActionResult> GetSubCategories(Guid id)
        {
            var categories = await _categoryService.GetSubCategories(id);
            return Ok(categories);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var category = await _categoryService.GetByID(id);
            return Ok(category);
        }

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


                return Ok(dbPath);
            }
            return Ok();
        }
    }
}