using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plants.Core.IRepositories
{
    public interface IProductRepository<U> where U : class
    {
        Task<U> Add(U product);

        Task<ICollection<U>> GetAll();

        Task<U> GetByID(Guid? ID);

        Task<ICollection<U>> GetByCategoryID(Guid? ID);

        Task<U> Update(U product);

        Task Delete(Guid? ID);
    }
}
