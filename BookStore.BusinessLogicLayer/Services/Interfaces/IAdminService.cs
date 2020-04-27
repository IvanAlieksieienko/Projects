using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.Models;
using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.Services.Interfaces
{
    public interface IAdminService
    {
        ICollection<AdminModel> GetAll();
        AdminModel GetById(int id);
        AdminModel GetByEmailPassword(string Email, string Password);
        AdminModel GetByEmail(string Email);
        void AddItem(AdminInputModel fields);
        void UpdateItem(int id, AdminInputModel fields);
        void Delete(int id);
    }
}
