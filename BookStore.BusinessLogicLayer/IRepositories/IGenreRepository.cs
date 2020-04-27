using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.Models;
using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.IRepositories
{
    public interface IGenreRepository
    {
        ICollection<GenreModel> GetAll();
        GenreModel GetById(int id);
        GenreModel CreateItem(GenreInputModel fields);
        GenreModel AddItem(GenreModel newGenre);
        void UpdateItem(int id, GenreInputModel fields);
        void Delete(int id);
    }
}
