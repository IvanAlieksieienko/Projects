using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Motopark.Core.IServices
{
    public interface ICategoryService<T> where T : class
    {
        List<T> allList { get; set; }
        Task<ICollection<T>> GetAll();
        Task<ICollection<T>> GetSubCategories(Guid id);
        Task<T> GetByID(Guid id);
        Task<T> Add(T item);
        Task<T> Update(T item);
        Task Delete(Guid id);
    }
}
