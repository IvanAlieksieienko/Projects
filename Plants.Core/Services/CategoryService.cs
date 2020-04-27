using Plants.Core.Entities;
using Plants.Core.IRepositories;
using Plants.Core.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plants.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository<Category> _categoryRepository;

        public CategoryService(ICategoryRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> Add(Category category)
        {
            return await _categoryRepository.Add(category);
        }

        public async Task<ICollection<Category>> GetAll()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<Category> GetByID(Guid? ID)
        {
            return await _categoryRepository.GetByID(ID);
        }

        public async Task<Category> Update(Category category)
        {
            return await _categoryRepository.Update(category);
        }

        public async Task Delete(Guid? ID)
        {
            await _categoryRepository.Delete(ID);
        }
    }
}
