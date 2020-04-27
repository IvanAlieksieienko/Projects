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
    public class DeliveryRepository : IDeliveryRepository<Delivery>
    {
        private string _connectionString;
        public DeliveryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Delivery> Add(Delivery item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Delivery newDelivery = new Delivery();
                newDelivery.ID = Guid.NewGuid();
                newDelivery.OrderID = item.OrderID;
                newDelivery.PayType = item.PayType;
                newDelivery.PostName = item.PostName;
                newDelivery.DeliveryName = item.DeliveryName;
                newDelivery.Region = item.Region;
                newDelivery.Street = item.Street;
                newDelivery.City = item.City;
                newDelivery.HouseNumber = item.HouseNumber;
                var sqlQuery = "insert into Deliveries (ID, OrderID, PayType, PostName, DeliveryName, Region, Street, City, HouseNumber) " +
                    "values (@ID, @OrderID, @PayType, @PostName, @DeliveryName, @Region, @Street, @City, @HouseNumber)";
                await db.ExecuteAsync(sqlQuery, newDelivery);
                return newDelivery;
            }
        }

        public async Task Delete(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "delete from Deliveries where ID = @id";
                await db.ExecuteAsync(sqlQuery, new { id });
            }
        }

        public async Task<ICollection<Delivery>> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return (await db.QueryAsync<Delivery>("select * from Deliveries")).ToList();
            }
        }

        public async Task<Delivery> GetByID(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return (await db.QueryAsync<Delivery>("select * from Deliveries where ID = @id", new { id })).FirstOrDefault();
            }
        }

        public async Task<Delivery> GetByOrderID(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return (await db.QueryAsync<Delivery>("select * from Deliveries where OrderID = @id", new { id })).FirstOrDefault();
            }
        }

        public async Task<Delivery> Update(Delivery item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Delivery newDelivery = new Delivery();
                newDelivery.ID = item.ID;
                newDelivery.OrderID = item.OrderID;
                newDelivery.PayType = item.PayType;
                newDelivery.PostName = item.PostName;
                newDelivery.DeliveryName = item.DeliveryName;
                newDelivery.Region = item.Region;
                newDelivery.Street = item.Street;
                newDelivery.City = item.City;
                newDelivery.HouseNumber = item.HouseNumber;
                var sqlQuery = "update Deliveries set " +
                    "OrderID = @OrderID, PayType = @PayType, PostName = @PostName, DeliveryName = @DeliveryName, " +
                    "Region = @Region, Street = @Street, City = @City, HouseNumber = @HouseNumber where ID = @ID";
                await db.ExecuteAsync(sqlQuery, newDelivery);
                return newDelivery;
            }
        }
    }
}
