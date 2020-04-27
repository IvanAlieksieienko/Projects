using AutoMapper;
using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.IRepositories;
using BookStore.BusinessLogicLayer.Models;
using BookStore.DataAccessLayer.Dapper.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BookStore.DataAccessLayer.Dapper.Repositories
{
    public class DapperBookRepository : IBookRepository
    {
        private string _connectionString;
        private IMapper _profile;

        public DapperBookRepository(string connectionString, IMapper mapper)
        {
            _connectionString = connectionString;
            _profile = mapper;
        }

        public ICollection<BookModel> GetAll()
        {
            var books = new Dictionary<int, Book>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = @"SELECT * FROM [Books] LEFT JOIN[AuthorBooks] ON([Books].[ID] = [AuthorBooks].[BookID]) 
                                LEFT JOIN[Authors] ON([Authors].[ID] = [AuthorBooks].[AuthorID]) 
                                LEFT JOIN[GenreBooks] ON([Books].[ID] = [GenreBooks].[BookID]) 
                                LEFT JOIN[Genres] ON([Genres].[ID] = [GenreBooks].[GenreID])";
                var result = db.Query<Book, AuthorBook, Author, GenreBook, Genre, Book>(sql, (book, authorBook, author, genreBook, genre) =>
                {
                    Book bookEntry;
                    if (!books.TryGetValue(book.ID, out bookEntry))
                    {
                        bookEntry = book;
                        bookEntry.AuthorBook = new List<AuthorBook>();
                        bookEntry.GenreBook = new List<GenreBook>();
                        books.Add(bookEntry.ID, bookEntry);
                    }
                    if (bookEntry.AuthorBook == null)
                    {
                        bookEntry.AuthorBook = new List<AuthorBook>();
                    }
                    if (authorBook != null && !bookEntry.AuthorBook.Any(ab => ab.ID == authorBook.ID))
                    {
                        authorBook.Book = book;
                        authorBook.Author = author;
                        bookEntry.AuthorBook.Add(authorBook);
                    }
                    if (bookEntry.GenreBook == null)
                    {
                        bookEntry.GenreBook = new List<GenreBook>();
                    }
                    if (genreBook != null && !bookEntry.GenreBook.Any(gb => gb.ID == genreBook.ID))
                    {
                        genreBook.Book = book;
                        genreBook.Genre = genre;
                        bookEntry.GenreBook.Add(genreBook);
                    }

                    return bookEntry;
                }, splitOn: "AuthorID,ID,BookID,ID").Distinct().ToList();
                return _profile.Map<List<Book>, List<BookModel>>(result);
            }
        }


        public BookModel GetById(int id)
        {
            return GetAll().FirstOrDefault(b => b.ID == id);
        }

        public BookModel CreateItem(BookInputModel fields)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var newBook = new Book();
                newBook.Title = fields.Title;
                newBook.ReleaseDate = fields.ReleaseDate;
                newBook.Price = fields.Price;
                AddItem(_profile.Map<Book, BookModel>(newBook));
                newBook = db.Query<Book>("SELECT * FROM Books WHERE Title = @Title", new { newBook.Title }).FirstOrDefault();
                newBook.AuthorBook = new List<AuthorBook>();
                newBook.GenreBook = new List<GenreBook>();
                var repositoryAuthor = new DapperAuthorRepository(_connectionString, _profile);
                var repositoryGenre = new DapperGenreRepository(_connectionString, _profile);
                var authorBooks = db.Query<AuthorBook>("SELECT * FROM AuthorBooks");
                var genreBooks = db.Query<GenreBook>("SELECT * FROM GenreBooks");
                foreach (var strAuthor in fields.Authors)
                {
                    Author newAuthor = _profile.Map<AuthorModel, Author>(repositoryAuthor.AddItem(repositoryAuthor.CreateItem(new AuthorInputModel(strAuthor))));
                    var authorBook = new AuthorBook();
                    int id = newBook.ID;
                    if (authorBooks.Where(ab => (ab.AuthorID == newAuthor.ID && ab.BookID == id)).FirstOrDefault() == null)
                    {
                        authorBook.AuthorID = newAuthor.ID;
                        authorBook.BookID = newBook.ID;
                        authorBook.Book = newBook;
                        authorBook.Author = newAuthor;
                        var sqlQuery = "INSERT INTO AuthorBooks (BookID, AuthorID) VALUES(@BookID, @AuthorID)";
                        db.Execute(sqlQuery, authorBook);
                        newBook.AuthorBook.Add(authorBook);
                    }
                }
                foreach (var strGenre in fields.Genres)
                {
                    Genre newGenre = _profile.Map<GenreModel, Genre>(repositoryGenre.AddItem(repositoryGenre.CreateItem(new GenreInputModel(strGenre))));
                    var genreBook = new GenreBook();
                    int id = newBook.ID;
                    if (genreBooks.Where(gb => (gb.GenreID == newGenre.ID && gb.BookID == id)).FirstOrDefault() == null)
                    {
                        genreBook.GenreID = newGenre.ID;
                        genreBook.BookID = newBook.ID;
                        genreBook.Book = newBook;
                        genreBook.Genre = newGenre;
                        var sqlQuery = "INSERT INTO GenreBooks (BookID, GenreID )VALUES(@BookID, @GenreID)";
                        db.Execute(sqlQuery, genreBook);
                        newBook.GenreBook.Add(genreBook);
                    }
                }
                return _profile.Map<Book, BookModel>(newBook);
            }
        }

        public BookModel AddItem(BookModel newBookModel)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                if (newBookModel == null)
                {
                    return null;
                }
                Book newBook = _profile.Map<BookModel, Book>(newBookModel);
                Book book = db.Query<Book>("SELECT * FROM Books WHERE Title = @Title", new { newBook.Title }).FirstOrDefault();
                if (book != null)
                {
                    return _profile.Map<Book, BookModel>(book);
                }
                string sqlQuery = sqlQuery = "INSERT INTO Books (Title, Price, ReleaseDate) VALUES(@Title, @Price, @ReleaseDate)";
                db.Execute(sqlQuery, newBook);
                book = db.Query<Book>("SELECT * FROM Books WHERE Title = @Title", new { newBook.Title }).FirstOrDefault();
                return _profile.Map<Book, BookModel>(book);
            }
        }

        public void UpdateItem(int id, BookInputModel fields)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Book thisBook = db.Query<Book>("SELECT * FROM Books WHERE ID = @id", new { id }).FirstOrDefault();
                if (thisBook != null)
                {
                    thisBook.Title = fields.Title;
                    thisBook.ReleaseDate = fields.ReleaseDate;
                    thisBook.Price = fields.Price;
                    var authorBooks = db.Query<AuthorBook>("SELECT * FROM AuthorBooks WHERE BookID = @ID", new { thisBook.ID }).ToList();
                    if (authorBooks != null)
                    {
                        for (int i = 0; i < authorBooks.Count; i++)
                        {
                            var sqlQuery4 = "DELETE FROM AuthorBooks WHERE BookID = @BookID";
                            db.Execute(sqlQuery4, new { authorBooks[i].BookID });
                        }
                    }
                    var genreBooks = db.Query<GenreBook>("SELECT * FROM GenreBooks WHERE BookID = @ID", new { thisBook.ID }).ToList();
                    if (genreBooks != null)
                    {
                        for (int i = 0; i < genreBooks.Count; i++)
                        {
                            var sqlQuery4 = "DELETE FROM GenreBooks WHERE BookID = @BookID";
                            db.Execute(sqlQuery4, new { genreBooks[i].BookID });
                        }
                    }

                    thisBook.AuthorBook = new List<AuthorBook>();
                    thisBook.GenreBook = new List<GenreBook>();
                    var repositoryAuthor = new DapperAuthorRepository(_connectionString, _profile);
                    var repositoryGenre = new DapperGenreRepository(_connectionString, _profile);
                    var authorBooks1 = db.Query<AuthorBook>("SELECT * FROM AuthorBooks");
                    var genreBooks1 = db.Query<GenreBook>("SELECT * FROM GenreBooks");
                    foreach (var strAuthor in fields.Authors)
                    {
                        Author newAuthor = _profile.Map<AuthorModel, Author>(repositoryAuthor.AddItem(repositoryAuthor.CreateItem(new AuthorInputModel(strAuthor))));
                        var authorBook = new AuthorBook();
                        if (authorBooks1.Where(ab => (ab.AuthorID == newAuthor.ID && ab.BookID == id)).FirstOrDefault() == null)
                        {
                            authorBook.AuthorID = newAuthor.ID;
                            authorBook.BookID = thisBook.ID;
                            authorBook.Book = thisBook;
                            authorBook.Author = newAuthor;
                            var sqlQuery1 = "INSERT INTO AuthorBooks (BookID, AuthorID) VALUES(@BookID, @AuthorID)";
                            db.Execute(sqlQuery1, authorBook);
                            thisBook.AuthorBook.Add(authorBook);
                        }
                    }
                    foreach (var strGenre in fields.Genres)
                    {
                        Genre newGenre = _profile.Map<GenreModel, Genre>(repositoryGenre.AddItem(repositoryGenre.CreateItem(new GenreInputModel(strGenre))));
                        var genreBook = new GenreBook();
                        if (genreBooks1.Where(gb => (gb.GenreID == newGenre.ID && gb.BookID == id)).FirstOrDefault() == null)
                        {
                            genreBook.GenreID = newGenre.ID;
                            genreBook.BookID = thisBook.ID;
                            genreBook.Book = thisBook;
                            genreBook.Genre = newGenre;
                            var sqlQuery2 = "INSERT INTO GenreBooks (BookID, GenreID )VALUES(@BookID, @GenreID)";
                            db.Execute(sqlQuery2, genreBook);
                            thisBook.GenreBook.Add(genreBook);
                        }
                    }
                    var sqlQuery = "UPDATE Books SET Title = @Title, Price = @Price, ReleaseDate = @ReleaseDate WHERE ID = @ID";
                    db.Execute(sqlQuery, thisBook);
                }
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Books WHERE ID = @id";
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}
