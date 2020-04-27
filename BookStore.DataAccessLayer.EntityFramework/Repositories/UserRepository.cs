using AutoMapper;
using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.IRepositories;
using BookStore.BusinessLogicLayer.Models;
using BookStore.DataAccessLayer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.DataAccessLayer.EntityFramework.Repositories
{
    public class UserRepository : IUserRepository
    {
        private BookContext _dbContext;
        private IMapper _profile;

        public UserRepository(BookContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _profile = mapper;
        }

        public ICollection<UserModel> GetAll()
        {
            var result = _dbContext.Users.Include(b => b.UserBook).ThenInclude(b => b.Book).Include(c => c.UserBasket).ThenInclude(b => b.Book).ToList();
            return _profile.Map<List<User>, List<UserModel>>(result);
        }

        public UserModel GetById(int id)
        {
            User user = _dbContext.Users.Include(b => b.UserBook).ThenInclude(b => b.Book).Include(c => c.UserBasket).ThenInclude(b => b.Book).ToList().FirstOrDefault(b => b.ID == id);
            if (user != null)
            {
                return _profile.Map<User, UserModel>(user);
            }
            return null;
        }

        public ICollection<UserBookModel> GetUserBooks(int id)
        {
            UserModel user = GetById(id);
            if (user != null)
            {
                return user.UserBook;
            }
            return null;
        }

        public ICollection<BasketModel> GetUserBasket(int id)
        {
            UserModel user = GetById(id);
            if (user != null)
            {
                return user.UserBasket;
            }
            return null;
        }

        public UserModel GetByEmailPassword(string Email, string Password)
        {
            User user = _dbContext.Users.Include(a => a.UserBook).Include(b => b.UserBasket).ToList().FirstOrDefault(u => u.Email == Email && u.Password == Password);
            if (user != null)
            {
                return _profile.Map<User, UserModel>(user);
            }
            return null;
        }

        public UserModel GetByEmail(string Email)
        {
            User user = _dbContext.Users.Include(a => a.UserBook).Include(b => b.UserBasket).ToList().FirstOrDefault(u => u.Email == Email);
            if (user != null)
            {
                return _profile.Map<User, UserModel>(user);
            }
            return null;
        }

        public UserModel CreateItem(UserInputModel fields)
        {
            var newUser = new User();
            newUser.Email = fields.Email;
            newUser.Password = fields.Password;
            return _profile.Map<User, UserModel>(newUser);
        }

        public UserModel AddItem(UserModel newUserModel)
        {
            if (newUserModel == null)
            {
                return null;
            }
            User newUser = _profile.Map<UserModel, User>(newUserModel);
            User user = _dbContext.Users.FirstOrDefault(a => a.Email == newUser.Email);
            if (user != null)
            {
                return _profile.Map<User, UserModel>(user);
            }
            user = newUser;
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return _profile.Map<User, UserModel>(user);
        }


        public void OrderBook(int id, int bookID)
        {
            User thisUser = _profile.Map<UserModel, User>(GetById(id));
            Book thisBook = _dbContext.Books.FirstOrDefault(b => b.ID == bookID);
            if (thisUser != null && thisBook != null)
            {
                Basket basket = _dbContext.UserBaskets.FirstOrDefault(b => b.ID == bookID && b.UserID == id);
                if (basket == null)
                {
                    var newBasket = new Basket();
                    newBasket.ID = bookID;
                    newBasket.BookID = bookID;
                    newBasket.UserID = id;
                    newBasket.User = thisUser;
                    newBasket.Book = thisBook;
                    _dbContext.UserBaskets.Add(newBasket);
                    _dbContext.SaveChanges();
                    basket = _dbContext.UserBaskets.FirstOrDefault(b => b.ID == bookID && b.UserID == id);
                }
                if (thisUser.UserBasket == null)
                {
                    thisUser.UserBasket = new List<Basket>();
                }
                thisUser.UserBasket.Add(basket);
                _dbContext.SaveChanges();
            }
        }

        public void DeleteOrderedBook(int id, int bookID)
        {
            User thisUser = _dbContext.Users.FirstOrDefault(u => u.ID == id);
            Basket basket = _dbContext.UserBaskets.FirstOrDefault(b => b.ID == bookID && b.UserID == id);
            if (thisUser != null && basket != null)
            {
                thisUser.UserBasket.Remove(basket);
                _dbContext.SaveChanges();
            }
        }

        public void BuyBook(int id, int bookID)
        {
            User thisUser = _profile.Map<UserModel, User>(GetById(id));
            Book thisBook = _dbContext.Books.FirstOrDefault(b => b.ID == bookID);
            if (thisUser != null && thisBook != null)
            {
                UserBook userBook = _dbContext.UserBooks.FirstOrDefault(b => b.ID == bookID && b.UserID == id);
                if (userBook == null)
                {
                    var newBook = new UserBook();
                    newBook.ID = bookID;
                    newBook.BookID = bookID;
                    newBook.UserID = id;
                    newBook.User = thisUser;
                    newBook.Book = thisBook;
                    _dbContext.UserBooks.Add(newBook);
                    _dbContext.SaveChanges();
                    userBook = _dbContext.UserBooks.FirstOrDefault(b => b.ID == bookID && b.UserID == id);
                }
                if (thisUser.UserBook == null)
                {
                    thisUser.UserBook = new List<UserBook>();
                }
                thisUser.UserBook.Add(userBook);
                _dbContext.SaveChanges();
            }
        }

        public void UpdateItem(int id, UserInputModel fields)
        {
            User thisUser = _dbContext.Users.FirstOrDefault(u => u.ID == id);
            if (thisUser != null)
            {
                thisUser.Email = fields.Email;
                thisUser.Password = fields.Password;
            }
            _dbContext.SaveChanges();
        }

        public void Confirm(int id)
        {
            User thisUser = _dbContext.Users.FirstOrDefault(u => u.ID == id);
            if (thisUser != null)
            {
                thisUser.Confirmed = true;
            }
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _dbContext.Users.Remove(_dbContext.Users.FirstOrDefault(u => u.ID == id));
            _dbContext.SaveChanges();
        }
    }
}
