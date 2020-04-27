using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Motopark.Core.IServices
{
    public interface IProductService<T> where T : class
    {
        List<T> allProducts { get; set; }
        Task<ICollection<T>> GetAll();
        Task<ICollection<T>> GetByCategoryID(Guid id);
        Task<ICollection<T>> GetProductsByListOfCategories(Guid[] ids);
        Task<T> GetByID(Guid id);
        Task<T> Add(T item);
        Task<T> Update(T item);
        Task Delete(Guid id);
    }
}
