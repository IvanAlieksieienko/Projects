using Motopark.Core.Entities;
using Motopark.Core.IRepositories;
using Motopark.Core.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Motopark.Core.Services
{
    public class AdminService : IAdminService<Admin>
    {
        private IAdminRepository<Admin> _adminRepository;

        public AdminService(IAdminRepository<Admin> adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Admin> Add(Admin item)
        {
            return await _adminRepository.Add(item);
        }

        public async Task Delete(Guid id)
        {
            await _adminRepository.Delete(id);
        }

        public async Task<Admin> GetByEmailPassword(string login, string password)
        {
            return await _adminRepository.GetByEmailPassword(login, password);
        }

        public async Task<Admin> GetByID(Guid id)
        {
            return await _adminRepository.GetByID(id);
        }

        public async Task<Admin> Update(Admin item)
        {
            return await _adminRepository.Update(item);
        }
    }
}
