using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.IRepositories;
using BookStore.BusinessLogicLayer.Models;
using BookStore.BusinessLogicLayer.Services.Interfaces;
using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.Services
{
    public class GenreService : IGenreService
    {
        IGenreRepository _repository;

        public GenreService(IGenreRepository repository)
        {
            _repository = repository;
        }

        public ICollection<GenreModel> GetAll()
        {
            return _repository.GetAll();
        }

        public GenreModel GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void AddItem(GenreInputModel inputModel)
        {
            var genre = _repository.CreateItem(inputModel);
            _repository.AddItem(genre);
        }

        public void UpdateItem(int id, GenreInputModel inputModel)
        {
            _repository.UpdateItem(id, inputModel);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
