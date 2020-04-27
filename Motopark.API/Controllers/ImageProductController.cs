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
    [Route("image")]
    [ApiController]
    public class ImageProductController : Controller
    {
        private IImageProductService<ImageProduct> _imageProductService;
        private ICategoryService<Category> _categoryService;
        private IProductService<Product> _productService;

        public ImageProductController(IImageProductService<ImageProduct> imageProductService, ICategoryService<Category> categoryService, IProductService<Product> productService)
        {
            _imageProductService = imageProductService;
            _categoryService = categoryService;
            _productService = productService;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByProductID(Guid id)
        {
            var images = await _imageProductService.GetByProductID(id);
            return Ok(images);
        }

        [HttpGet("files/product/{id:guid}")]
        public async Task<IActionResult> GetByFilesImageProductID(Guid id)
        {
            var imageProducts = (await _imageProductService.GetByProductID(id)).ToList();
            var listFileBytes = new List<byte[]>();
            for (int i = 0; i < imageProducts.Count; i++)
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(imageProducts[i].ImagePath);
                listFileBytes.Add(fileBytes);
            }
            return Ok(listFileBytes);
        }

        [HttpGet("files/category/{id:guid}")]
        public async Task<IActionResult> GetByFilesCategoryID(Guid id)
        {
            var imagePath = (await _categoryService.GetByID(id)).ImagePath;
            byte[] fileBytes = System.IO.File.ReadAllBytes(imagePath);
            string fileName = Path.GetFileName(imagePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}