using AutoMapper;
using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.IRepositories;
using BookStore.BusinessLogicLayer.Models;
using BookStore.DataAccessLayer.Dapper.Entities;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BookStore.DataAccessLayer.Dapper.Repositories
{
    public class DapperGenreRepository : IGenreRepository
    {
        private string _connectionString;
        private IMapper _profile;

        public DapperGenreRepository(string connectionString, IMapper mapper)
        {
            _connectionString = connectionString;
            _profile = mapper;
        }

        public ICollection<GenreModel> GetAll()
        {
            var genres = new Dictionary<int, Genre>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM [Genres] LEFT JOIN [GenreBooks] ON ([Genres].[ID] = [GenreBooks].[GenreID]) LEFT JOIN [Books] ON ([GenreBooks].[BookID] = [Books].[ID])";

                var result = db.Query<Genre, GenreBook, Book, Genre>(sql, (genre, genreBook, book) =>
                {
                    Genre genreEntry;
                    if (!genres.TryGetValue(genre.ID, out genreEntry))
                    {
                        genreEntry = genre;
                        genreEntry.GenreBook = new List<GenreBook>();
                        genres.Add(genreEntry.ID, genreEntry);
                    }
                    if (book != null)
                    {
                        genreBook.Book = book;
                    }
                    if (genreBook == null)
                    {
                        genreEntry.GenreBook = new List<GenreBook>();
                    }
                    genreEntry.GenreBook.Add(genreBook);

                    return genreEntry;
                }, splitOn: "BookID,ID").Distinct().ToList();

                return _profile.Map<List<Genre>, List<GenreModel>>(result);
            }
        }

        public GenreModel GetById(int id)
        {
            return GetAll().FirstOrDefault(g => g.ID == id);
        }

        public GenreModel CreateItem(GenreInputModel fields)
        {
            var newGenre = new Genre();
            newGenre.Name = fields.Name;
            return _profile.Map<Genre, GenreModel>(newGenre);
        }

        public GenreModel AddItem(GenreModel newGenreModel)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                if (newGenreModel == null)
                {
                    return null;
                }
                Genre newGenre = _profile.Map<GenreModel, Genre>(newGenreModel);
                Genre genre = db.Query<Genre>("SELECT * FROM Genres WHERE Name = @Name", new { newGenre.Name }).FirstOrDefault();
                if (genre != null)
                {
                    return _profile.Map<Genre, GenreModel>(genre);
                }
                var sqlQuery = "INSERT INTO Genres (Name) VALUES(@Name)";
                db.Execute(sqlQuery, newGenre);
                genre = db.Query<Genre>("SELECT * FROM Genres WHERE Name = @Name", new { newGenre.Name }).FirstOrDefault();
                return _profile.Map<Genre, GenreModel>(genre);
            }
        }

        public void UpdateItem(int id, GenreInputModel fields)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Genre genre = db.Query<Genre>("SELECT * FROM Genres WHERE ID = @id", new { id }).FirstOrDefault();
                if (genre != null)
                {
                    genre.Name = fields.Name;
                    var sqlQuery = "UPDATE Genres SET Name = @Name WHERE ID = @ID";
                    db.Execute(sqlQuery, genre);
                }
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Genres WHERE ID = @id";
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}
