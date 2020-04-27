using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Motopark.Core.IRepositories
{
    public interface IProductRepository<T> where T : class
    {
        Task<ICollection<T>> GetAll();
        Task<ICollection<T>> GetByCategoryID(Guid id);
        Task<T> GetByID(Guid id);
        Task<T> Add(T item);
        Task<T> Update(T item);
        Task Delete(Guid id);
    }
}
