using Motopark.Core.Entities;
using Motopark.Core.IRepositories;
using Motopark.Core.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Motopark.Core.Services
{
    public class CategoryService : ICategoryService<Category>
    {
        private ICategoryRepository<Category> _categoryRepository;

        public List<Category> allList { get; set; }

        [InjectionConstructor]
        public CategoryService(ICategoryRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> Add(Category item)
        {
            return await _categoryRepository.Add(item);
        }

        public async Task Delete(Guid id)
        {
            await _categoryRepository.Delete(id);
        }

        public async Task<ICollection<Category>> GetAll()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<Category> GetByID(Guid id)
        {
            return await _categoryRepository.GetByID(id);
        }

        public async Task<ICollection<Category>> GetSubCategories(Guid id)
        {
            allList = new List<Category>();
            await GetAllSubCategories(id);
            return allList;
        }

        public async Task GetAllSubCategories(Guid id)
        {
            var categories = await _categoryRepository.GetSubCategories(id);
            foreach(var category in categories)
            {
                allList.Add(category);
                await GetAllSubCategories(category.ID);
            }
        }
            
        public async Task<Category> Update(Category item)
        {
            return await _categoryRepository.Update(item);
        }
    }
}
