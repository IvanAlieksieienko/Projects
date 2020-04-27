using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plants.Core.IRepositories
{
    public interface IAdminRepository<U> where U :class
    {
        Task<U> Add(U admin);

        Task<ICollection<U>> GetAll();

        Task<U> GetByID(Guid? ID);

        Task<U> GetByLoginPassword(string login, string password);

        Task<U> Update(U admin);

        Task Delete(Guid? ID);
    }
}
