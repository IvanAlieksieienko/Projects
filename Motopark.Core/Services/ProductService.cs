using Motopark.Core.Entities;
using Motopark.Core.IRepositories;
using Motopark.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motopark.Core.Services
{
    public class ProductService : IProductService<Product>
    {
        private IProductRepository<Product> _productRepository;

        public List<Product> allProducts { get; set; }

        public ProductService(IProductRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Add(Product item)
        {
            return await _productRepository.Add(item);
        }

        public async Task Delete(Guid id)
        {
            await _productRepository.Delete(id);
        }

        public async Task<ICollection<Product>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public async Task<ICollection<Product>> GetByCategoryID(Guid id)
        {
            return await _productRepository.GetByCategoryID(id);
        }

        public async Task<ICollection<Product>> GetProductsByListOfCategories(Guid[] ids)
        {
            allProducts = new List<Product>();
            var products = await _productRepository.GetAll();
            foreach(var id in ids)
            {
                var localProducts = products.Where(p => p.CategoryID == id);
                foreach(var product in localProducts)
                {
                    allProducts.Add(product);
                }
            }
            return allProducts;
        }

        public async Task<Product> GetByID(Guid id)
        {
            return await _productRepository.GetByID(id);
        }

        public async Task<Product> Update(Product item)
        {
            return await _productRepository.Update(item);
        }
    }
}
