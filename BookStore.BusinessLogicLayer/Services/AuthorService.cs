using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.IRepositories;
using BookStore.BusinessLogicLayer.Models;
using BookStore.BusinessLogicLayer.Services.Interfaces;
using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.Services
{
    public class AuthorService : IAuthorService
    {
        IAuthorRepository _repository;

        public AuthorService(IAuthorRepository repository)
        {
            _repository = repository;
        }

        public ICollection<AuthorModel> GetAll()
        {
            return _repository.GetAll();
        }

        public AuthorModel GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void AddItem(AuthorInputModel inputModel)
        {
            var author = _repository.CreateItem(inputModel);
            _repository.AddItem(author);
        }

        public void UpdateItem(int id, AuthorInputModel inputModel)
        {
            _repository.UpdateItem(id, inputModel);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
