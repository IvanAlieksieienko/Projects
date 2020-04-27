using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.Models;
using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.Services.Interfaces
{
    public interface IUserService
    {
        ICollection<UserModel> GetAll();
        UserModel GetById(int id);
        UserModel GetByEmailPassword(string Email, string Password);
        UserModel GetByEmail(string Email);
        ICollection<UserBookModel> GetUserBooks(int id);
        ICollection<BasketModel> GetUserBasket(int id);
        void OrderBook(int id, int BookID);
        void DeleteOrderedBook(int id, int bookID);
        void BuyBook(int id, int bookID);
        void AddItem(UserInputModel fields);
        void UpdateItem(int id, UserInputModel fields);
        void Confirm(int id);
        void Delete(int id);
    }
}
