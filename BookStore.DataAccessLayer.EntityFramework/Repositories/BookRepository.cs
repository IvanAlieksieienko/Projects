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
    public class BookRepository : IBookRepository
    {
        private BookContext _dbContext;
        private IMapper _profile;

        public BookRepository(BookContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _profile = mapper;
        }

        public ICollection<BookModel> GetAll()
        {
            var result = _dbContext.Books.Include(b => b.AuthorBook).ThenInclude(a => a.Author).Include(b => b.GenreBook).ThenInclude(g => g.Genre).ToList();
            return _profile.Map<List<Book>, List<BookModel>>(result);
        }


        public BookModel GetById(int id)
        {
            Book book = _dbContext.Books.Include(b => b.AuthorBook).ThenInclude(a => a.Author).Include(b => b.GenreBook).ThenInclude(g => g.Genre).FirstOrDefault(b => b.ID == id);
            if (book != null)
            {
                return _profile.Map<Book, BookModel>(book);
            }
            return null;
        }

        public BookModel CreateItem(BookInputModel fields)
        {
            var newBook = new Book();
            newBook.Title = fields.Title;
            newBook.ReleaseDate = fields.ReleaseDate;
            newBook.Price = fields.Price;
            newBook.AuthorBook = new List<AuthorBook>();
            newBook.GenreBook = new List<GenreBook>();
            var repositoryAuthor = new AuthorRepository(_dbContext, _profile);
            var repositoryGenre = new GenreRepository(_dbContext, _profile);
            foreach (var strAuthor in fields.Authors)
            {
                Author newAuthor = _profile.Map<AuthorModel, Author>(repositoryAuthor.AddItem(repositoryAuthor.CreateItem(new AuthorInputModel(strAuthor))));
                var authorBook = new AuthorBook();
                authorBook.AuthorID = newAuthor.ID;
                authorBook.BookID = newBook.ID;
                authorBook.Book = newBook;
                authorBook.Author = newAuthor;
                _dbContext.AuthorBooks.Add(authorBook);
                newBook.AuthorBook.Add(authorBook);
            }
            foreach (var strGenre in fields.Genres)
            {
                Genre newGenre = _profile.Map<GenreModel, Genre>(repositoryGenre.AddItem(repositoryGenre.CreateItem(new GenreInputModel(strGenre))));
                var genreBook = new GenreBook();
                genreBook.GenreID = newGenre.ID;
                genreBook.BookID = newBook.ID;
                genreBook.Book = newBook;
                genreBook.Genre = newGenre;
                _dbContext.GenreBooks.Add(genreBook);
                newGenre.GenreBook.Add(genreBook);
            }
            _dbContext.SaveChanges();
            return _profile.Map<Book, BookModel>(newBook);
        }

        public BookModel AddItem(BookModel newBookModel)
        {
            if (newBookModel == null)
            {
                return null;
            }
            Book newBook = _profile.Map<BookModel, Book>(newBookModel);
            Book book = _dbContext.Books.FirstOrDefault(a => a.Title == newBook.Title);
            if (book != null)
            {
                return _profile.Map<Book, BookModel>(book);
            }
            book = newBook;
            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();
            return _profile.Map<Book, BookModel>(book);
        }

        public void UpdateItem(int id, BookInputModel fields)
        {
            Book thisBook = _dbContext.Books.FirstOrDefault(b => b.ID == id);
            if (thisBook != null)
            {
                thisBook.Title = fields.Title;
                thisBook.ReleaseDate = fields.ReleaseDate;
                thisBook.Price = fields.Price;
                var authorbooks = _dbContext.AuthorBooks.Where(b => b.BookID == thisBook.ID).ToList();
                if (authorbooks != null)
                {
                    _dbContext.AuthorBooks.RemoveRange(authorbooks);
                }
                var genrebooks = _dbContext.GenreBooks.Where(b => b.BookID == thisBook.ID).ToList();
                if (genrebooks != null)
                {
                    _dbContext.GenreBooks.RemoveRange(genrebooks);
                }
                thisBook.AuthorBook = new List<AuthorBook>();
                thisBook.GenreBook = new List<GenreBook>();
                var repositoryAuthor = new AuthorRepository(_dbContext, _profile);
                var repositoryGenre = new GenreRepository(_dbContext, _profile);
                if (fields.Authors.Length > 0)
                {
                    foreach (var author in fields.Authors)
                    {
                        Author newAuthor = _profile.Map<AuthorModel, Author>(repositoryAuthor.AddItem(repositoryAuthor.CreateItem(new AuthorInputModel(author))));
                        var authorBook = new AuthorBook();
                        authorBook.AuthorID = newAuthor.ID;
                        authorBook.BookID = thisBook.ID;
                        authorBook.Book = thisBook;
                        authorBook.Author = newAuthor;
                        _dbContext.AuthorBooks.Add(authorBook);
                        thisBook.AuthorBook.Add(authorBook);
                    }
                }
                foreach (var genre in fields.Genres)
                {
                    Genre newGenre = _profile.Map<GenreModel, Genre>(repositoryGenre.AddItem(repositoryGenre.CreateItem(new GenreInputModel(genre))));
                    var genreBook = new GenreBook();
                    genreBook.GenreID = newGenre.ID;
                    genreBook.BookID = thisBook.ID;
                    genreBook.Book = thisBook;
                    genreBook.Genre = newGenre;
                    _dbContext.GenreBooks.Add(genreBook);
                    newGenre.GenreBook.Add(genreBook);
                }
            }
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _dbContext.Books.Remove(_dbContext.Books.FirstOrDefault(b => b.ID == id));
            _dbContext.SaveChanges();
        }
    }
}
