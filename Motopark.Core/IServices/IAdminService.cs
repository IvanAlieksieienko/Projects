using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Motopark.Core.IServices
{
    public interface IAdminService<T> where T : class
    {
        Task<T> GetByID(Guid id);
        Task<T> GetByEmailPassword(string login, string password);
        Task<T> Add(T item);
        Task<T> Update(T item);
        Task Delete(Guid id);
    }
}
