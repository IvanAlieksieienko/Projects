using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.Models;
using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.IRepositories
{
    public interface IUserRepository
    {
        ICollection<UserModel> GetAll();
        UserModel GetById(int id);
        UserModel GetByEmailPassword(string Email, string Password);
        UserModel GetByEmail(string Email);
        ICollection<UserBookModel> GetUserBooks(int id);
        ICollection<BasketModel> GetUserBasket(int id);
        UserModel CreateItem(UserInputModel fields);
        UserModel AddItem(UserModel newUser);
        void BuyBook(int userId, int bookId);
        void OrderBook(int userId, int bookId);
        void UpdateItem(int id, UserInputModel fields);
        void Confirm(int id);
        void Delete(int id);
        void DeleteOrderedBook(int id, int bookID);
    }
}
