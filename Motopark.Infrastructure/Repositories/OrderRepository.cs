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
    public class OrderRepository : IOrderRepository<Order>
    {
        private string _connectionString;
        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Order> Add(Order item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Order newDelivery = new Order();
                newDelivery.ID = Guid.NewGuid();
                newDelivery.BasketID = item.BasketID;
                newDelivery.DeliveryID = item.DeliveryID;
                newDelivery.Email = item.Email;
                newDelivery.Comment = item.Comment;
                newDelivery.Name = item.Name;
                newDelivery.Patronymic = item.Patronymic;
                newDelivery.PhoneNumber = item.PhoneNumber;
                newDelivery.Surname = item.Surname;
                newDelivery.TotalPrice = item.TotalPrice;
                newDelivery.CreationTime = item.CreationTime;
                newDelivery.OrderState = item.OrderState;
                var sqlQuery = "insert into Orders (ID, BasketID, DeliveryID, Email, Comment, Name, Patronymic, Surname, TotalPrice, PhoneNumber, CreationTime, OrderState) " +
                    "values (@ID, @BasketID, @DeliveryID, @Email, @Comment, @Name, @Patronymic, @Surname, @TotalPrice, @PhoneNumber, @CreationTime, @OrderState)";
                await db.ExecuteAsync(sqlQuery, newDelivery);
                return newDelivery;
            }
        }

        public async Task Delete(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "delete from Orders where ID = @id";
                await db.ExecuteAsync(sqlQuery, new { id });
            }
        }

        public async Task<ICollection<Order>> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return (await db.QueryAsync<Order>("select * from Orders")).ToList();
            }
        }

        public async Task<Order> GetByID(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return (await db.QueryAsync<Order>("select * from Orders where ID = @id", new { id })).FirstOrDefault();
            }
        }

        public async Task<Order> Update(Order item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Order newDelivery = new Order();
                newDelivery.ID = item.ID;
                newDelivery.BasketID = item.BasketID;
                newDelivery.DeliveryID = item.DeliveryID;
                newDelivery.Email = item.Email;
                newDelivery.Comment = item.Comment;
                newDelivery.Name = item.Name;
                newDelivery.Patronymic = item.Patronymic;
                newDelivery.PhoneNumber = item.PhoneNumber;
                newDelivery.Surname = item.Surname;
                newDelivery.TotalPrice = item.TotalPrice;
                newDelivery.CreationTime = item.CreationTime;
                newDelivery.OrderState = item.OrderState;
                var sqlQuery = "update Orders set BasketID = @BasketID, " +
                    "DeliveryID = @DeliveryID, " +
                    "Email = @Email, " +
                    "Comment = @Comment, " +
                    "Name = @Name, " +
                    "Patronymic = @Patronymic, " +
                    "Surname = @Surname, " +
                    "PhoneNumber = @PhoneNumber, " +
                    "CreationTime = @CreationTime, " +
                    "OrderState = @OrderState, " +
                    "TotalPrice = @TotalPrice where ID = @ID";
                await db.ExecuteAsync(sqlQuery, newDelivery);
                return newDelivery;
            }
        }
    }
}