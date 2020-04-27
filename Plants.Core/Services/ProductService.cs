using Plants.Core.Entities;
using Plants.Core.IRepositories;
using Plants.Core.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plants.Core.Services
{
    public class ProductService : IProductService
    {
        private IProductRepository<Product> _productRepository;

        public ProductService(IProductRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Add(Product product)
        {
            return await _productRepository.Add(product);
        }

        public async Task<ICollection<Product>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public async Task<Product> GetByID(Guid? ID)
        {
            return await _productRepository.GetByID(ID);
        }

        public async Task<ICollection<Product>> GetByCategoryID(Guid? ID)
        {
            return await _productRepository.GetByCategoryID(ID);
        }

        public async Task<Product> Update(Product product)
        {
            return await _productRepository.Update(product);
        }

        public async Task Delete(Guid? ID)
        {
           await _productRepository.Delete(ID);
        }
    }
}
