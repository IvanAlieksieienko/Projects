using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.Models;
using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.Services.Interfaces
{
    public interface IBookService
    {
        ICollection<BookModel> GetAll();
        BookModel GetById(int id);
        void AddItem(BookInputModel fields);
        void UpdateItem(int id, BookInputModel fields);
        void Delete(int id);
    }
}
