using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.IRepositories;
using BookStore.BusinessLogicLayer.Models;
using BookStore.BusinessLogicLayer.Services.Interfaces;
using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public ICollection<UserModel> GetAll()
        {
            return _repository.GetAll();
        }

        public UserModel GetById(int id)
        {
            return _repository.GetById(id);
        }

        public UserModel GetByEmailPassword(string Email, string Password)
        {
            return _repository.GetByEmailPassword(Email, Password);
        }

        public UserModel GetByEmail(string Email)
        {
            return _repository.GetByEmail(Email);
        }

        public ICollection<UserBookModel> GetUserBooks(int id)
        {
            return _repository.GetUserBooks(id);
        }

        public ICollection<BasketModel> GetUserBasket(int id)
        {
            return _repository.GetUserBasket(id);
        }

        public void OrderBook(int id, int BookID)
        {
            _repository.OrderBook(id, BookID);
        }

        public void DeleteOrderedBook(int id, int bookID)
        {
            _repository.DeleteOrderedBook(id, bookID);
        }

        public void BuyBook(int id, int bookID)
        {
            _repository.BuyBook(id, bookID);
        }

        public void AddItem(UserInputModel inputModel)
        {
            var author = _repository.CreateItem(inputModel);
            _repository.AddItem(author);
        }

        public void UpdateItem(int id, UserInputModel inputModel)
        {
            _repository.UpdateItem(id, inputModel);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public void Confirm(int id)
        {
            _repository.Confirm(id);
        }
    }
}
