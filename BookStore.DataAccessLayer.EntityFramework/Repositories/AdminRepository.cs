using AutoMapper;
using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.IRepositories;
using BookStore.BusinessLogicLayer.Models;
using BookStore.DataAccessLayer.EntityFramework.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.DataAccessLayer.EntityFramework.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private BookContext _dbContext;
        private IMapper _profile;

        public AdminRepository(BookContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _profile = mapper;
        }

        public ICollection<AdminModel> GetAll()
        {
            var admins = _dbContext.Admins.ToList();
            if (admins != null)
            {
                return _profile.Map<List<Admin>, ICollection<AdminModel>>(admins);
            }
            return null;
        }

        public AdminModel GetById(int id)
        {
            Admin admin = _dbContext.Admins.FirstOrDefault(a => a.ID == id);
            if (admin != null)
            {
                return _profile.Map<Admin, AdminModel>(admin);
            }
            return null;
        }

        public AdminModel GetByEmailPassword(string Email, string Password)
        {
            Admin admin = _dbContext.Admins.FirstOrDefault(a => a.Email == Email && a.Password == Password);
            if (admin != null)
            {
                return _profile.Map<Admin, AdminModel>(admin);
            }
            return null;
        }

        public AdminModel GetByEmail(string Email)
        {
            Admin admin = _dbContext.Admins.FirstOrDefault(a => a.Email == Email);
            if (admin != null)
            {
                return _profile.Map<Admin, AdminModel>(admin);
            }
            return null;
        }

        public AdminModel CreateItem(AdminInputModel fields)
        {
            var newAdmin = new Admin();
            newAdmin.Email = fields.Email;
            newAdmin.Password = fields.Password;
            return _profile.Map<Admin, AdminModel>(newAdmin);
        }

        public AdminModel AddItem(AdminModel newAdminModel)
        {
            if (newAdminModel == null)
            {
                return null;
            }
            Admin newAdmin = _profile.Map<AdminModel, Admin>(newAdminModel);
            Admin admin = _dbContext.Admins.FirstOrDefault(a => a.Email == newAdmin.Email);
            if (admin != null)
            {
                return _profile.Map<Admin, AdminModel>(admin);
            }
            admin = newAdmin;
            _dbContext.Admins.Add(admin);
            _dbContext.SaveChanges();
            return _profile.Map<Admin, AdminModel>(admin);
        }

        public void UpdateItem(int id, AdminInputModel fields)
        {
            Admin thisAdmin = _dbContext.Admins.FirstOrDefault(a => a.ID == id);
            if (thisAdmin != null)
            {
                thisAdmin.Email = fields.Email;
                thisAdmin.Password = fields.Password;
            }
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _dbContext.Admins.Remove(_dbContext.Admins.FirstOrDefault(a => a.ID == id));
            _dbContext.SaveChanges(); 
        }
    }
}
