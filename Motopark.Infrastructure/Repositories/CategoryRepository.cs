using Dapper;
using Motopark.Core.Entities;
using Motopark.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motopark.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository<Category>
    {
        private string _connectionString;
        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Category> Add(Category item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Category newCategory = new Category();
                newCategory.ID = Guid.NewGuid();
                newCategory.CategoryID = item.CategoryID;
                newCategory.Name = item.Name;
                newCategory.Description = item.Description;
                newCategory.ImagePath = item.ImagePath;
                var sqlQuery = "insert into Categories (ID, CategoryID, Name, Description, ImagePath) values (@ID, @CategoryID, @Name, @Description, @ImagePath)";
                await db.ExecuteAsync(sqlQuery, newCategory);
                return newCategory;
            }
        }

        public async Task Delete(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "delete from Categories where ID = @id";
                await db.ExecuteAsync(sqlQuery, new { id });
            }
        }

        public async Task<ICollection<Category>> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return (await db.QueryAsync<Category>("select * from Categories")).ToList();
            }
        }

        public async Task<Category> GetByID(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return (await db.QueryAsync<Category>("select * from Categories where ID = @id", new { id })).FirstOrDefault();
            }
        }

        public async Task<ICollection<Category>> GetSubCategories(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return (await db.QueryAsync<Category>("select * from Categories where CategoryID = @id", new { id })).ToList();
            }
        }

        public async Task<Category> Update(Category item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Category newCategory = new Category();
                newCategory.ID = item.ID;
                newCategory.CategoryID = item.CategoryID;
                newCategory.Name = item.Name;
                newCategory.Description = item.Description;
                newCategory.ImagePath = item.ImagePath;
                var sqlQuery = "update Categories set CategoryID = @CategoryID, Name = @Name, Description = @Description, ImagePath = @ImagePath where ID = @ID";
                await db.ExecuteAsync(sqlQuery, newCategory);
                return newCategory;
            }
        }
    }
}
