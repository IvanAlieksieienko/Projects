using Motopark.Core.Entities;
using Motopark.Core.IRepositories;
using Motopark.Core.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Motopark.Core.Services
{
    public class ImageProductService : IImageProductService<ImageProduct>
    {
        private IImageProductRepository<ImageProduct> _imageProductRepository;

        public ImageProductService(IImageProductRepository<ImageProduct> imageProductRepository)
        {
            _imageProductRepository = imageProductRepository;
        }

        public async Task<ImageProduct> Add(ImageProduct item)
        {
            return await _imageProductRepository.Add(item);
        }

        public async Task<ImageProduct> GetByID(Guid id)
        {
            return await _imageProductRepository.GetByID(id);
        }

        public async Task Delete(Guid id)
        {
            await _imageProductRepository.Delete(id);
        }

        public async Task<ICollection<ImageProduct>> GetByProductID(Guid id)
        {
            return await _imageProductRepository.GetByProductID(id);
        }

        public async Task<ImageProduct> Update(ImageProduct item)
        {
            return await _imageProductRepository.Update(item);
        }
    }
}
