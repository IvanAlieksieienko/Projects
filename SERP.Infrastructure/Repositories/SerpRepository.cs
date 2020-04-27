using Dapper;
using SERP.Core.IRepositories;
using SERP.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Repositories
{
    public class SerpRepository : ISerpRepository
    {
        private string _connectionString;

        public SerpRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task Add(TaskModel task)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Tasks (ID, SearchEngine, LocationCode, LocationName, Domain, Keywords, Position)" +
                    "VALUES (@ID, @SearchEngine, @LocationCode, @LocationName, @Domain, @Keywords, @Position)";
                await db.ExecuteAsync(sqlQuery, task);
            }
        }

        public async Task Update(TaskModel task)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "Update Tasks SET SearchEngine = @SearchEngine, LocationCode = @LocationCode, LocationName = @LocationName, " +
                    "Domain = @Domain, Keywords = @Keywords, Position = @Position WHERE ID = @ID";
                await db.ExecuteAsync(sqlQuery, task);
            }
        }
    }
}
