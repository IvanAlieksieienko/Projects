using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Motopark.Core.IServices
{
    public interface IFeatureService<T> where T : class
    {
        Task<T> GetByID(Guid id);
        Task<ICollection<T>> GetByProductID(Guid id);
        Task AddFeatures(List<T> features);
        Task Add(T feature);
        Task Delete(Guid id);
        Task DeleteByProductID(Guid id);
        Task<T> Update(T feature);
        Task UpdateFeatures(List<T> features);
    }
}
