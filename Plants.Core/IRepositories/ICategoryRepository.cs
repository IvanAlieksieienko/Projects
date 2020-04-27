using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plants.Core.IRepositories
{
    public interface ICategoryRepository<U> where U : class
    {
        Task<U> Add(U category);

        Task<ICollection<U>> GetAll();

        Task<U> GetByID(Guid? ID);

        Task<U> Update(U category);

        Task Delete(Guid? ID);
    }
}
