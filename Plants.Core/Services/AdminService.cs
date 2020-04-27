using Plants.Core.Entities;
using Plants.Core.IRepositories;
using Plants.Core.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plants.Core.Services
{
    public class AdminService : IAdminService
    {
        private IAdminRepository<Admin> _adminRepository;

        public AdminService(IAdminRepository<Admin> adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Admin> GetByLoginPassword(string login, string password)
        {
            return await _adminRepository.GetByLoginPassword(login, password);
        }

        public async Task<Admin> Add(Admin admin)
        {
            return await _adminRepository.Add(admin);
        }

        public async Task<ICollection<Admin>> GetAll()
        {
            return await _adminRepository.GetAll();
        }

        public async Task<Admin> GetByID(Guid? ID)
        {
            return await _adminRepository.GetByID(ID);
        }

        public async Task<Admin> Update(Admin admin)
        {
            return await _adminRepository.Update(admin);
        }

        public async Task Delete(Guid? ID)
        {
            await _adminRepository.Delete(ID);
        }
    }
}
