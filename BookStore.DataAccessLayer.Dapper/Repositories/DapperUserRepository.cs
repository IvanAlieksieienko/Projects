using AutoMapper;
using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.IRepositories;
using BookStore.BusinessLogicLayer.Models;
using BookStore.DataAccessLayer.Dapper.Entities;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BookStore.DataAccessLayer.Dapper.Repositories
{
    public class DapperUserRepository : IUserRepository
    {
        private string _connectionString;
        private IMapper _profile;

        public DapperUserRepository(string connectionString, IMapper mapper)
        {
            _connectionString = connectionString;
            _profile = mapper;
        }

        public UserModel AddItem(UserModel newUserModel)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                if (newUserModel == null)
                {
                    return null;
                }
                User newUser = _profile.Map<UserModel, User>(newUserModel);
                User user = db.Query<User>("SELECT * FROM Users WHERE ID = @id", new { newUser.ID }).FirstOrDefault();
                if (user != null)
                {
                    return _profile.Map<User, UserModel>(user);
                }
                var sqlQuery = "INSERT INTO Users (Email, Password) VALUES(@Email, @Password)";
                db.Execute(sqlQuery, newUser);
                user = db.Query<User>("SELECT * FROM Users WHERE Email = @Email", new { newUser.Email }).FirstOrDefault();
                return _profile.Map<User, UserModel>(user);
            }
        }

        public void OrderBook(int id, int BookID)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                User thisUser = db.Query<User>("SELECT * FROM Users WHERE ID = @id", new { id }).FirstOrDefault();
                Book thisBook = db.Query<Book>("SELECT * FROM Books WHERE ID = @BookID", new { BookID }).FirstOrDefault();
                if (thisUser != null && thisBook != null)
                {
                    Basket basket = db.Query<Basket>("SELECT * FROM UserBaskets WHERE BookID = @BookID and UserID = @id", new { BookID, id }).FirstOrDefault();
                    if (basket == null)
                    {
                        var newBasket = new Basket();
                        newBasket.BookID = BookID;
                        newBasket.UserID = id;
                        newBasket.User = thisUser;
                        newBasket.Book = thisBook;
                        var sqlQuery1 = "INSERT INTO UserBaskets (BookID, UserID) VALUES(@BookID, @UserID)";
                        db.Execute(sqlQuery1, newBasket);
                        basket = db.Query<Basket>("SELECT * FROM UserBaskets WHERE BookID = @BookID and UserID = @id", new { BookID, id }).FirstOrDefault();
                    }

                }
            }
        }

        public void BuyBook(int id, int BookID)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                User thisUser = db.Query<User>("SELECT * FROM Users WHERE ID = @id", new { id }).FirstOrDefault();
                Book thisBook = db.Query<Book>("SELECT * FROM Books WHERE ID = @BookID", new { BookID }).FirstOrDefault();
                if (thisUser != null && thisBook != null)
                {
                    UserBook userBook = db.Query<UserBook>("SELECT * FROM UserBooks WHERE BookID = @BookID and UserID = @id", new { BookID, id }).FirstOrDefault();
                    if (userBook == null)
                    {
                        var newUserBook = new UserBook();
                        newUserBook.BookID = BookID;
                        newUserBook.UserID = id;
                        newUserBook.User = thisUser;
                        newUserBook.Book = thisBook;
                        var sqlQuery1 = "INSERT INTO UserBooks (BookID, UserID) VALUES(@BookID, @UserID)";
                        db.Execute(sqlQuery1, newUserBook);
                        userBook = db.Query<UserBook>("SELECT * FROM UserBooks WHERE BookID = @BookID and UserID = @id", new { BookID, id }).FirstOrDefault();
                    }

                    var sqlQuery2 = "DELETE FROM UserBaskets WHERE BookID = @BookID and UserID = @id";
                    db.Execute(sqlQuery2, new { BookID, id });
                }
            }
        }

        public void DeleteOrderedBook(int id, int BookID)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                User thisUser = db.Query<User>("SELECT * FROM Users WHERE ID = @id", new { id }).FirstOrDefault();
                Basket basket = db.Query<Basket>("SELECT * FROM UserBaskets WHERE BookID = @BookID and UserID = @id", new { BookID, id }).FirstOrDefault();
                if (thisUser != null && basket != null)
                {
                    var sqlQuery = "DELETE FROM UserBaskets WHERE BookID = @BookID and UserID = @id";
                    db.Execute(sqlQuery, new { BookID, id });
                }
            }
        }

        public void DeleteBoughtBook(int id, int BookID)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                User thisUser = db.Query<User>("SELECT * FROM Users WHERE ID = @id", new { id }).FirstOrDefault();
                UserBook userBook = db.Query<UserBook>("SELECT *FROM UserBooks WHERE BookID = @BookID and UserID = @id", new { BookID, id }).FirstOrDefault();
                if (thisUser != null && userBook != null)
                {
                    thisUser.UserBook.Remove(userBook);
                    var sqlQuery = "DELETE FROM UserBooks WHERE BookID = @BookID and UserID = @id";
                    db.Execute(sqlQuery, new { BookID, id });
                }
            }
        }

        public UserModel CreateItem(UserInputModel fields)
        {
            var newUser = new User();
            newUser.Email = fields.Email;
            newUser.Password = fields.Password;
            return _profile.Map<User, UserModel>(newUser);
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Users WHERE ID = @id";
                db.Execute(sqlQuery, new { id });
            }
        }

        public ICollection<UserModel> GetAll()
        {
            var users = new Dictionary<int, User>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = @"SELECT * FROM [Users]	LEFT JOIN[UserBaskets] ON([Users].[ID] = [UserBaskets].[UserID]) 
                        LEFT JOIN[UserBooks] ON([Users].[ID] = [UserBooks].[UserID]) 
                        LEFT JOIN[Books] ON([Books].[ID] = [UserBooks].[BookID] or [Books].[ID] = [UserBaskets].[BookID] ) ";
                var result = db.Query<User, Basket, UserBook, Book, User>(sql, (user, basket, userBook, book) =>
                {
                    User userEntry;
                    if (!users.TryGetValue(user.ID, out userEntry))
                    {
                        userEntry = user;
                        userEntry.UserBasket = new List<Basket>();
                        userEntry.UserBook = new List<UserBook>();
                        users.Add(userEntry.ID, userEntry);
                    }
                    if (userEntry.UserBook == null)
                    {
                        userEntry.UserBook = new List<UserBook>();
                    }
                    if (userBook != null && !userEntry.UserBook.Any(ab => ab.BookID == userBook.BookID))
                    {
                        userBook.Book = book;
                        userBook.User = user;
                        userEntry.UserBook.Add(userBook);
                    }
                    if (userEntry.UserBasket == null)
                    {
                        userEntry.UserBasket = new List<Basket>();
                    }
                    if (basket != null && !userEntry.UserBasket.Any(gb => gb.BookID == basket.BookID))
                    {
                        basket.Book = book;
                        basket.User = user;
                        userEntry.UserBasket.Add(basket);
                    }

                    return userEntry;
                }, splitOn: "UserID,BookID,ID").Distinct().ToList();
                return _profile.Map<List<User>, List<UserModel>>(result);
            }
        }

        public UserModel GetByEmailPassword(string Email, string Password)
        {
            return GetAll().FirstOrDefault(u => (u.Email == Email && u.Password == Password));
        }

        public UserModel GetByEmail(string Email)
        {
            return GetAll().FirstOrDefault(u => u.Email == Email);
        }

        public UserModel GetById(int id)
        {
            return GetAll().FirstOrDefault(u => u.ID == id);
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

        public void UpdateItem(int id, UserInputModel fields)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                User user = db.Query<User>("SELECT * FROM Users WHERE ID = @id", new { id }).FirstOrDefault();
                if (user != null)
                {
                    user.Email = fields.Email;
                    user.Password = fields.Password;
                    var sqlQuery = "UPDATE Users SET Email = @Email, Password = @Password WHERE ID = @ID";
                    db.Execute(sqlQuery, user);
                }
            }
        }

        public void Confirm(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                User user = db.Query<User>("SELECT * FROM Users WHERE ID = @id", new { id }).FirstOrDefault();
                if (user != null)
                {
                    user.Confirmed = true;
                    var sqlQuery = "UPDATE Users SET Confirmed = @Confirmed WHERE ID = @ID";
                    db.Execute(sqlQuery, user);
                }
            }
        }
    }
}
