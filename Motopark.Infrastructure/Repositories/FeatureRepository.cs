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
    public class FeatureRepository : IFeatureRepository<Feature>
    {
        private string _connectionString;

        public FeatureRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Feature> GetByID(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var feature = (await db.QueryAsync<Feature>("SELECT * FROM Features WHERE ID = @id", new { id })).FirstOrDefault();
                return feature;
            }
        }

        public async Task<ICollection<Feature>> GetByProductID(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var features = (await db.QueryAsync<Feature>("SELECT * FROM Features WHERE ProductID = @id ORDER BY Position ASC", new { id })).ToList();
                return features;
            }
        }

        public async Task Add(Feature feature)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Feature newFeature = new Feature();
                newFeature.ID = Guid.NewGuid();
                newFeature.ProductID = feature.ProductID;
                newFeature.FeatureName = feature.FeatureName;
                newFeature.FeatureValue = feature.FeatureValue;
                newFeature.Position = feature.Position;
                var sqlQuery = "INSERT INTO Features (ID, ProductID, FeatureName, FeatureValue, Position) VALUES (@ID, @ProductID, @FeatureName, @FeatureValue, @Position)";
                await db.ExecuteAsync(sqlQuery, newFeature);
            }
        }

        public async Task Delete(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                await db.QueryAsync("DELETE FROM Features WHERE ID = @id", new { id });
            }
        }

        public async Task DeleteByProductID(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                await db.QueryAsync("DELETE FROM Features WHERE ProductID = @id", new { id });

            }
        }

        public async Task<Feature> Update(Feature feature)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Feature newFeature = new Feature() { ID = feature.ID, ProductID = feature.ProductID, FeatureName = feature.FeatureName, FeatureValue = feature.FeatureValue, Position = feature.Position };
                var sqlQuery = "UPDATE Features SET ProductID = @ProductID, FeatureName = @FeatureName, FeatureValue = @FeatureValue, Position = @Position WHERE ID = @ID";
                await db.ExecuteAsync(sqlQuery, newFeature);
                return newFeature;
            }
        }
    }
}
