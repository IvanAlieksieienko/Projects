using Dapper;
using Plants.Core.Entities;
using Plants.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plants.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository<Category>
    {
        private string _connectionString;

        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Category> Add(Category category)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Category newCategory = new Category();
                newCategory.ID = Guid.NewGuid();
                newCategory.Name = category.Name;
                newCategory.Description = category.Description;
                newCategory.ImagePath = category.ImagePath;
                var sqlQuery = "INSERT INTO Category (ID, Name, Description, ImagePath) " +
                    "VALUES (@ID, @Name, @Description, @ImagePath)";
                await db.ExecuteAsync(sqlQuery, newCategory);
                return await GetByID(newCategory.ID);
            }
        }

        public async Task<ICollection<Category>> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                List<Category> categories = (await db.QueryAsync<Category>("SELECT * FROM Category")).ToList();
                return categories;
            }
        }

        public async Task<Category> GetByID(Guid? ID)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Category category = (await db.QueryAsync<Category>("SELECT * FROM Category WHERE ID = @ID", new { ID })).FirstOrDefault();
                return category;
            }
        }

        public async Task<Category> Update(Category category)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Category newCategory = new Category();
                newCategory.ID = category.ID;
                newCategory.Name = category.Name;
                newCategory.Description = category.Description;
                newCategory.ImagePath = category.ImagePath;
                var sqlQuery = "UPDATE Category SET " +
                                "ID = @ID, " +
                                "Name = @Name, " +
                                "Description = @Description, " +
                                "ImagePath = @ImagePath " +
                                "WHERE ID = @ID";
                await db.ExecuteAsync(sqlQuery, newCategory);
                return await GetByID(newCategory.ID);
            }
        }

        public async Task Delete(Guid? ID)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Category WHERE ID = @ID";
                await db.ExecuteAsync(sqlQuery, new { ID });
            }
        }
    }
}
