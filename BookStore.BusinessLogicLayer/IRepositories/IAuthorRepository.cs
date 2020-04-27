using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.Models;
using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.IRepositories
{
    public interface IAuthorRepository
    {
        ICollection<AuthorModel> GetAll();
        AuthorModel GetById(int id);
        AuthorModel CreateItem(AuthorInputModel fields);
        AuthorModel AddItem(AuthorModel newAuthor);
        void UpdateItem(int id, AuthorInputModel fields);
        void Delete(int id);
    }
}
