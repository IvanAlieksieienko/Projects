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
    public class DapperAuthorRepository : IAuthorRepository
    {
        private string _connectionString;
        private IMapper _profile;

        public DapperAuthorRepository(string connectionString, IMapper mapper)
        {
            _connectionString = connectionString;
            _profile = mapper;
        }

        public ICollection<AuthorModel> GetAll()
        {
            var authors = new Dictionary<int, Author>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM [Authors] LEFT JOIN [AuthorBooks] ON ([Authors].[ID] = [AuthorBooks].[AuthorID]) LEFT JOIN [Books] ON ([AuthorBooks].[BookID] = [Books].[ID])";
                var result = db.Query<Author, AuthorBook, Book, Author>(sql, (author, authorBook, book) =>
                {
                    Author authorEntry;
                    if (!authors.TryGetValue(author.ID, out authorEntry))
                    {
                        authorEntry = author;
                        authorEntry.AuthorBook = new List<AuthorBook>();
                        authors.Add(authorEntry.ID, authorEntry);
                    }
                    if (book != null)
                    {
                        authorBook.Book = book;
                    }
                    if (authorBook != null)
                    {
                        authorEntry.AuthorBook.Add(authorBook);
                    }
                    return authorEntry;
                }, splitOn: "AuthorID,ID").Distinct().ToList();

                return _profile.Map<List<Author>, List<AuthorModel>>(result);
            }

        }

        public AuthorModel GetById(int id)
        {
            return GetAll().FirstOrDefault(a => a.ID == id);
        }

        public AuthorModel CreateItem(AuthorInputModel fields)
        {
            var newAuthor = new Author();
            newAuthor.Name = fields.Name;
            return _profile.Map<Author, AuthorModel>(newAuthor);
        }

        public AuthorModel AddItem(AuthorModel newAuthorModel)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                if (newAuthorModel == null)
                {
                    return null;
                }
                Author newAuthor = _profile.Map<AuthorModel, Author>(newAuthorModel);
                Author author = db.Query<Author>("SELECT * FROM Authors WHERE Name = @name", new { newAuthor.Name }).FirstOrDefault();
                if (author != null)
                {
                    return _profile.Map<Author, AuthorModel>(author);
                }
                var sqlQuery = "INSERT INTO Authors (Name) VALUES(@Name)"; ;
                db.Execute(sqlQuery, newAuthor);
                author = db.Query<Author>("SELECT * FROM Authors WHERE Name = @name", new { newAuthor.Name }).FirstOrDefault();
                return _profile.Map<Author, AuthorModel>(author);
            }
        }

        public void UpdateItem(int id, AuthorInputModel fields)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Author author = db.Query<Author>("SELECT * FROM Authors WHERE ID = @id", new { id }).FirstOrDefault();
                if (author != null)
                {
                    author.Name = fields.Name;
                    var sqlQuery = "UPDATE Authors SET Name = @Name WHERE ID = @ID";
                    db.Execute(sqlQuery, author);
                }
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Authors WHERE ID = @id";
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}
