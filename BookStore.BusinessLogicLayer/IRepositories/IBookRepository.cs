using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.Models;
using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.IRepositories
{
    public interface IBookRepository
    {
        ICollection<BookModel> GetAll();
        BookModel GetById(int id);
        BookModel CreateItem(BookInputModel fields);
        BookModel AddItem(BookModel newBookModel);
        void UpdateItem(int id, BookInputModel fields);
        void Delete(int id);
    }
}
