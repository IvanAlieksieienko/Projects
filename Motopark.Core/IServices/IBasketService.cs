using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Motopark.Core.IServices
{
    public interface IBasketService<T> where T : class
    {
        Task<ICollection<T>> GetAll();
        Task<ICollection<T>> GetByBasketID(Guid id);
        Task<T> Add(T item);
        Task<T> ChangeCount(Guid basketId, Guid productId, int count);
        Task Delete(Guid id);
        Task DeleteByProduct(Guid id, Guid basketId);
    }
}
