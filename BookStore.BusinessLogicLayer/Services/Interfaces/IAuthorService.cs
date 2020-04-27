using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.Models;
using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.Services.Interfaces
{
    public interface IAuthorService
    {
        ICollection<AuthorModel> GetAll();
        AuthorModel GetById(int id);
        void AddItem(AuthorInputModel fields);
        void UpdateItem(int id, AuthorInputModel fields);
        void Delete(int id);
    }
}
