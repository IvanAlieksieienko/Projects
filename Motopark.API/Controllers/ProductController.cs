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
    [Route("product")]
    [ApiController]
    public class ProductController : Controller
    {
        private IProductService<Product> _productService;
        
        public ProductController(IProductService<Product> productService)
        {
            _productService = productService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAll();
            return Ok(products);
        }

        [HttpGet("category/{id:guid}")]
        public async Task<IActionResult> GetByCategoryID(Guid id)
        {
            var products = await _productService.GetByCategoryID(id);
            return Ok(products);
        }

        [HttpPost("category/all")]
        public async Task<IActionResult> GetProductsByListOfCategories(Guid[] ids)
        {
            var products = await _productService.GetProductsByListOfCategories(ids);
            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var product = await _productService.GetByID(id);
            return Ok(product);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage()
        {
            var file = Request.Form.Files[0];
            var folderName = Path.Combine("Resources", "Images", "Product");
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