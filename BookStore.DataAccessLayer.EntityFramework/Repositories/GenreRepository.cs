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
    public class GenreRepository : IGenreRepository
    {
        private BookContext _dbContext;
        private IMapper _profile;

        public GenreRepository(BookContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _profile = mapper;
        }

        public ICollection<GenreModel> GetAll()
        {
            var result = _dbContext.Genres.Include(b => b.GenreBook).ThenInclude(b => b.Book).ToList();
            return _profile.Map<List<Genre>, List<GenreModel>>(result);
        }

        public GenreModel GetById(int id)
        {
            Genre genre = _dbContext.Genres.Include(a => a.GenreBook).ThenInclude(b => b.Book).FirstOrDefault(b => b.ID == id);
            if (genre != null)
            {
                return _profile.Map<Genre, GenreModel>(genre);
            }
            return null;
        }

        public GenreModel CreateItem(GenreInputModel fields)
        {
            var newGenre = new Genre();
            newGenre.Name = fields.Name;
            return _profile.Map<Genre, GenreModel>(newGenre);
        }

        public GenreModel AddItem(GenreModel newGenreModel)
        {
            if (newGenreModel == null)
            {
                return null;
            }
            Genre newGenre = _profile.Map<GenreModel, Genre>(newGenreModel);
            Genre genre = _dbContext.Genres.FirstOrDefault(a => a.Name == newGenre.Name);
            if (genre != null)
            {
                return _profile.Map<Genre, GenreModel>(genre);
            }
            genre = newGenre;
            _dbContext.Genres.Add(genre);
            _dbContext.SaveChanges();
            return _profile.Map<Genre, GenreModel>(genre);
        }

        public void UpdateItem(int id, GenreInputModel fields)
        {
            Genre thisGenre = _dbContext.Genres.FirstOrDefault(g => g.ID == id);
            if (thisGenre != null)
            {
                thisGenre.Name = fields.Name;
            }
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _dbContext.Genres.Remove(_dbContext.Genres.FirstOrDefault(g => g.ID == id));
            _dbContext.SaveChanges();
        }
    }
}

