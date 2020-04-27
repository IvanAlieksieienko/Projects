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
    public class BasketRepository : IBasketRepository<Basket>
    {
        private string _connectionString;

        public BasketRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Basket> Add(Basket item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Basket newBasket = new Basket();
                newBasket.ID = item.ID;
                newBasket.ProductID = item.ProductID;
                newBasket.Count = item.Count;
                var sqlQuery = "insert into Baskets (ID, ProductID, Count) values (@ID, @ProductID, @Count)";
                await db.ExecuteAsync(sqlQuery, newBasket);
                return newBasket;
            }
        }

        public async Task<Basket> ChangeCount(Guid basketId, Guid productId, int count)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "update Baskets set Count = @count where ID = @basketId and ProductID = @productId";
                await db.ExecuteAsync(sqlQuery, new { count, basketId, productId });
                return (await db.QueryAsync<Basket>("select * from Baskets where ID = @basketId", new { basketId })).FirstOrDefault();
            }
        }

        public async Task Delete(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "delete from Baskets where ID = @id";
                await db.ExecuteAsync(sqlQuery, new { id });
            }
        }

        public async Task DeleteByProduct(Guid id, Guid basketId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "delete from Baskets where ProductID = @id and ID = @basketId";
                await db.ExecuteAsync(sqlQuery, new { id, basketId });
            }
        }

        public async Task<ICollection<Basket>> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return (await db.QueryAsync<Basket>("select * from Baskets")).ToList();
            }
        }

        public async Task<ICollection<Basket>> GetByBasketID(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return (await db.QueryAsync<Basket>("select * from Baskets where ID = @id", new { id })).ToList();
            }
        }
    }
}
