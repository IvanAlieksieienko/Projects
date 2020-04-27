using Plants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plants.Core.IServices
{
    public interface ICategoryService
    {
        Task<Category> Add(Category category);
        Task<ICollection<Category>> GetAll();
        Task<Category> GetByID(Guid? ID);
        Task<Category> Update(Category category);
        Task Delete(Guid? ID);
    }
}
