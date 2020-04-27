using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.Models;
using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.Services.Interfaces
{
    public interface IGenreService
    {
        ICollection<GenreModel> GetAll();
        GenreModel GetById(int id);
        void AddItem(GenreInputModel fields);
        void UpdateItem(int id, GenreInputModel fields);
        void Delete(int id);
    }
}
