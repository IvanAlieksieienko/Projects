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
    public class DapperAdminRepository : IAdminRepository
    {
        private string _connectionString;
        private IMapper _profile;

        public DapperAdminRepository(string connectionString, IMapper mapper)
        {
            _connectionString = connectionString;
            _profile = mapper;
            //AddItem(new AdminModel { Email = "admin@gmail.com", Password = "admin" });
        }

        public ICollection<AdminModel> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var admins = db.Query<Admin>("SELECT * FROM Admins").ToList();
                if (admins != null)
                {
                    return _profile.Map<List<Admin>, ICollection<AdminModel>>(admins);
                }
                return null;
            }
        }

        public AdminModel GetById(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Admin admin = db.Query<Admin>("SELECT * FROM Admins WHERE ID = @id", new { id }).FirstOrDefault();
                if (admin != null)
                {
                    return _profile.Map<Admin, AdminModel>(admin);
                }
                return null;
            }
        }

        public AdminModel GetByEmailPassword(string Email, string Password)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Admin admin = db.Query<Admin>("SELECT * FROM Admins WHERE Email = @Email and Password = @Password", new { Email, Password }).FirstOrDefault();
                if (admin != null)
                {
                    return _profile.Map<Admin, AdminModel>(admin);
                }
                return null;
            }
        }

        public AdminModel GetByEmail(string Email)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Admin admin = db.Query<Admin>("SELECT * FROM Admins WHERE Email = @Email", new { Email }).FirstOrDefault();
                if (admin != null)
                {
                    return _profile.Map<Admin, AdminModel>(admin);
                }
                return null;
            }
        }

        public AdminModel CreateItem(AdminInputModel fields)
        {
            var newAdmin = new Admin();
            newAdmin.Email = fields.Email;
            newAdmin.Password = fields.Password;
            return _profile.Map<Admin, AdminModel>(newAdmin);
        }

        public AdminModel AddItem(AdminModel newAdminModel)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                if (newAdminModel == null)
                {
                    return null;
                }
                Admin newAdmin = _profile.Map<AdminModel, Admin>(newAdminModel);
                Admin admin = db.Query<Admin>("SELECT * FROM Admins WHERE Email = @Email", new { newAdmin.Email }).FirstOrDefault();
                if (admin != null)
                {
                    return _profile.Map<Admin, AdminModel>(admin);
                }
                var sqlQuery = "INSERT INTO Admins (Email, Password) VALUES(@Email, @Password)";
                db.Execute(sqlQuery, newAdmin);
                admin = db.Query<Admin>("SELECT * FROM Admins WHERE Email = @Email", new { newAdmin.Email }).FirstOrDefault();
                return _profile.Map<Admin, AdminModel>(admin);
            }
        }

        public void UpdateItem(int id, AdminInputModel fields)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Admin admin = db.Query<Admin>("SELECT * FROM Admins WHERE ID = @id", new { id }).FirstOrDefault();
                if (admin != null)
                {
                    admin.Email = fields.Email;
                    admin.Password = fields.Password;
                    var sqlQuery = "UPDATE Admins SET Email = @Email, Password = @Password WHERE ID = @ID";
                    db.Execute(sqlQuery, admin);
                }
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Admins WHERE ID = @id";
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}
