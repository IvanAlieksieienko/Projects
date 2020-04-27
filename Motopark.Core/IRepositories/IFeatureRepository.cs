using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Motopark.Core.IRepositories
{
    public interface IFeatureRepository<T> where T : class
    {
        Task<T> GetByID(Guid id);
        Task<ICollection<T>> GetByProductID(Guid id);
        Task Add(T feature);
        Task Delete(Guid id);
        Task DeleteByProductID(Guid id);
        Task<T> Update(T feature);
    }
}
