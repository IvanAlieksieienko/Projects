using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.IRepositories;
using BookStore.BusinessLogicLayer.Models;
using BookStore.BusinessLogicLayer.Services.Interfaces;
using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.Services
{
    public class AdminService : IAdminService
    {
        IAdminRepository _repository;

        public AdminService(IAdminRepository repository)
        {
            _repository = repository;
        }

        public ICollection<AdminModel> GetAll()
        {
            return _repository.GetAll();
        }

        public AdminModel GetById(int id)
        {
            return _repository.GetById(id);
        }

        public AdminModel GetByEmailPassword(string Email, string Password)
        {
            return _repository.GetByEmailPassword(Email, Password);
        }

        public AdminModel GetByEmail(string Email)
        {
            return _repository.GetByEmail(Email);
        }

        public void AddItem(AdminInputModel inputModel)
        {
            var admin = _repository.CreateItem(inputModel);
            _repository.AddItem(admin);
        }

        public void UpdateItem(int id, AdminInputModel inputModel)
        {
            _repository.UpdateItem(id, inputModel);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
