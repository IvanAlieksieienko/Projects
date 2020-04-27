using Plants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plants.Core.IServices
{
    public interface IAdminService
    {
        Task<Admin> GetByLoginPassword(string login, string password);
        Task<Admin> Add(Admin admin);
        Task<ICollection<Admin>> GetAll();
        Task<Admin> GetByID(Guid? ID);
        Task<Admin> Update(Admin admin);
        Task Delete(Guid? ID);
    }
}
