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
    public class ProductRepository : IProductRepository<Product>
    {
        private string _connectionString;
        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Product> Add(Product item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Product newProduct = new Product();
                newProduct.ID = Guid.NewGuid();
                newProduct.CategoryID = item.CategoryID;
                newProduct.Name = item.Name;
                newProduct.Description = item.Description;
                newProduct.Price = item.Price;
                newProduct.IsAvailable = item.IsAvailable;
                var sqlQuery = "insert into Products (ID, CategoryID, Name, Description, Price, IsAvailable) values (@ID, @CategoryID, @Name, @Description, @Price, @IsAvailable)";
                await db.ExecuteAsync(sqlQuery, newProduct);
                return newProduct;
            }
        }

        public async Task Delete(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "delete from Products where ID = @id";
                await db.ExecuteAsync(sqlQuery, new { id });
            }
        }

        public async Task<ICollection<Product>> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return (await db.QueryAsync<Product>("select * from Products")).ToList();
            }
        }

        public async Task<Product> GetByID(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return (await db.QueryAsync<Product>("select * from Products where ID = @id", new { id })).FirstOrDefault();
            }
        }

        public async Task<ICollection<Product>> GetByCategoryID(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return (await db.QueryAsync<Product>("select * from Products where CategoryID = @id", new { id })).ToList();
            }
        }

        public async Task<Product> Update(Product item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Product newProduct = new Product();
                newProduct.ID = item.ID;
                newProduct.CategoryID = item.CategoryID;
                newProduct.Name = item.Name;
                newProduct.Description = item.Description;
                newProduct.Price = item.Price;
                newProduct.IsAvailable = item.IsAvailable;
                var sqlQuery = "update Products set CategoryID = @CategoryID, Name = @Name, Description = @Description, Price = @Price, IsAvailable = @IsAvailable where ID = @ID";
                await db.ExecuteAsync(sqlQuery, newProduct);
                return newProduct;
            }
        }
    }
}
