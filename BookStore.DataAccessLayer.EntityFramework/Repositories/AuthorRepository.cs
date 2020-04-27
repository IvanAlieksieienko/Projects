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
    public class AuthorRepository : IAuthorRepository
    {
        private BookContext _dbContext;
        private IMapper _profile;

        public AuthorRepository(BookContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _profile = mapper;
        }

        public ICollection<AuthorModel> GetAll()
        {
            var result = _dbContext.Authors.Include(b => b.AuthorBook).ThenInclude(b => b.Book).ToList();
            return _profile.Map<List<Author>, List<AuthorModel>>(result);
        }

        public AuthorModel GetById(int id)
        {
            Author author = _dbContext.Authors.Include(a => a.AuthorBook).ThenInclude(b => b.Book).FirstOrDefault(b => b.ID == id);
            if (author != null)
            {
                return _profile.Map<Author, AuthorModel>(author);
            }
            return null;
        }

        public AuthorModel CreateItem(AuthorInputModel fields)
        {
            var newAuthor = new Author();
            newAuthor.Name = fields.Name;
            return _profile.Map<Author, AuthorModel>(newAuthor);
        }

        public AuthorModel AddItem(AuthorModel newAuthorModel)
        {
            if (newAuthorModel == null)
            {
                return null;
            }
            Author newAuthor = _profile.Map<AuthorModel, Author>(newAuthorModel);
            Author author = _dbContext.Authors.FirstOrDefault(a => a.Name == newAuthor.Name);
            if (author !=  null)
            {
                return _profile.Map<Author, AuthorModel>(author);
            }
            author = newAuthor;
            _dbContext.Authors.Add(author);
            _dbContext.SaveChanges();

            return _profile.Map<Author, AuthorModel>(author);
        }

        public void UpdateItem(int id, AuthorInputModel fields)
        {
            Author thisAuthor = _dbContext.Authors.FirstOrDefault(a=> a.ID == id);
            if (thisAuthor != null)
            {
                thisAuthor.Name = fields.Name;
            }
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _dbContext.Authors.Remove(_dbContext.Authors.FirstOrDefault(a => a.ID == id));
            _dbContext.SaveChanges();
        }
    }
}
