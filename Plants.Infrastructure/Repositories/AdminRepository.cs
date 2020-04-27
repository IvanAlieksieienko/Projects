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
    public class AdminRepository : IAdminRepository<Admin>
    {
        private string _connectionString;

        public AdminRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Admin> Add(Admin admin)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Admin newAdmin = new Admin();
                newAdmin.ID = Guid.NewGuid();
                newAdmin.Login = admin.Login;
                newAdmin.Password = admin.Password;
                var sqlQuery = "INSERT INTO Admin (ID, Login, Password) " +
                    "VALUES (@ID, @Login, @Password)";
                await db.ExecuteAsync(sqlQuery, newAdmin);
                return await GetByID(newAdmin.ID);
            }
        }

        public async Task<ICollection<Admin>> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                List<Admin> categories = (await db.QueryAsync<Admin>("SELECT * FROM Admin")).ToList();
                return categories;
            }
        }

        public async Task<Admin> GetByID(Guid? ID)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Admin admin = (await db.QueryAsync<Admin>("SELECT * FROM Admin WHERE ID = @ID", new { ID })).FirstOrDefault();
                return admin;
            }
        }

        public async Task<Admin> Update(Admin admin)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Admin newAdmin = new Admin();
                newAdmin.ID = admin.ID;
                newAdmin.Login = admin.Login;
                newAdmin.Password = admin.Password;
                var sqlQuery = "UPDATE Admin SET " +
                                "ID = @ID, " +
                                "Login = @Login, " +
                                "Password = @Password " +
                                "WHERE ID = @ID";
                await db.ExecuteAsync(sqlQuery, newAdmin);
                return await GetByID(newAdmin.ID);
            }
        }

        public async Task Delete(Guid? ID)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Admin WHERE ID = @ID";
                await db.ExecuteAsync(sqlQuery, new { ID });
            }
        }

        public async Task<Admin> GetByLoginPassword(string login, string password)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Admin admin = (await db.QueryAsync<Admin>("SELECT * FROM Admin WHERE Login = @login and Password = @password", new { login, password })).FirstOrDefault();
                return admin;
            }
        }
    }
}
