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
    public class ImageProductRepository : IImageProductRepository<ImageProduct>
    {
        private string _connectionString;
        public ImageProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ImageProduct> Add(ImageProduct item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                ImageProduct imageProduct = new ImageProduct();
                imageProduct.ID = Guid.NewGuid();
                imageProduct.ProductID = item.ProductID;
                imageProduct.ImagePath = item.ImagePath;
                imageProduct.IsFirst = item.IsFirst;
                var sqlQuery = "insert into ImageProducts (ID, ProductID, ImagePath, IsFirst) values (@ID, @ProductID, @ImagePath, @IsFirst)";
                await db.ExecuteAsync(sqlQuery, imageProduct);
                return imageProduct;
            }
        }

        public async Task<ImageProduct> GetByID(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return (await db.QueryAsync<ImageProduct>("select * from ImageProducts where ID = @id", new { id })).FirstOrDefault();
            }
        }

        public async Task Delete(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                await db.ExecuteAsync("delete from ImageProducts where ID = @id", new { id });
            }
        }

        public async Task<ICollection<ImageProduct>> GetByProductID(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return (await db.QueryAsync<ImageProduct>("select * from ImageProducts where ProductID = @id", new { id })).ToList();
            }    
        }

        public async Task<ImageProduct> Update(ImageProduct item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                ImageProduct imageProduct = new ImageProduct();
                imageProduct.ID = item.ID;
                imageProduct.ProductID = item.ProductID;
                imageProduct.ImagePath = item.ImagePath;
                imageProduct.IsFirst = item.IsFirst;
                var sqlQuery = "update ImageProducts set ProductID = @ProductID, ImagePath = @ImagePath, IsFirst=@IsFirst where ID = @ID";
                await db.ExecuteAsync(sqlQuery, imageProduct);
                return imageProduct;
            }
        }
    }
}
