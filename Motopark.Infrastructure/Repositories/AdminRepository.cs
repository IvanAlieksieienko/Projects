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
    public class AdminRepository : IAdminRepository<Admin>
    {
        private string _connectionString;

        public AdminRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Admin> Add(Admin item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Admin newAdmin = new Admin();
                newAdmin.ID = Guid.NewGuid();
                newAdmin.Login = item.Login;
                newAdmin.Password = item.Password;
                var sqlQuery = "insert into Admins (ID, Login, Password) " +
                    "values (@ID, @Login, @Password)";
                await db.ExecuteAsync(sqlQuery, newAdmin);
                return newAdmin;
            }
        }

        public async Task Delete(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "delete from Admins where ID = @id";
                await db.ExecuteAsync(sqlQuery, new { id });
            }
        }

        public async Task<Admin> GetByEmailPassword(string login, string password)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var admin = (await db.QueryAsync<Admin>("select * from Admins where Login = @login and Password = @password", new { login, password })).FirstOrDefault();
                return admin;
            }
        }

        public async Task<Admin> GetByID(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var admin = (await db.QueryAsync<Admin>("select * from Admins where ID = @id", new { id })).FirstOrDefault();
                return admin;
            }
        }

        public async Task<Admin> Update(Admin item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Admin newAdmin = new Admin();
                newAdmin.ID = item.ID;
                newAdmin.Login = item.Login;
                newAdmin.Password = item.Password;
                var sqlQuery = "update Admins set Login = @Login, Password = @Password where ID = @ID";
                await db.ExecuteAsync(sqlQuery, item);
                return newAdmin;
            }
        }
    }
}
