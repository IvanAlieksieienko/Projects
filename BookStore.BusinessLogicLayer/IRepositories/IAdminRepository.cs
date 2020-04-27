using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.Models;
using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.IRepositories
{
    public interface IAdminRepository
    {
        ICollection<AdminModel> GetAll();
        AdminModel GetById(int id);
        AdminModel GetByEmail(string Email);
        AdminModel GetByEmailPassword(string Email, string Password);
        AdminModel CreateItem(AdminInputModel fields);
        AdminModel AddItem(AdminModel newAdmin);
        void UpdateItem(int id, AdminInputModel fields);
        void Delete(int id);
    }
}
