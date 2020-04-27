using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.IRepositories;
using BookStore.BusinessLogicLayer.Models;
using BookStore.BusinessLogicLayer.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.Services
{
    public class BookService : IBookService
    {
        private IBookRepository _repository;

        public BookService(IBookRepository repository)
        {
            _repository = repository;
        }

        public ICollection<BookModel> GetAll()
        {
            return _repository.GetAll();
        }

        public BookModel GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void AddItem(BookInputModel inputModel)
        {
            var book = _repository.CreateItem(inputModel);
            _repository.AddItem(book);
        }

        public void UpdateItem(int id, BookInputModel inputModel)
        {
            _repository.UpdateItem(id, inputModel);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
