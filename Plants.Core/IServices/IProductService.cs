using Plants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plants.Core.IServices
{
    public interface IProductService
    {
        Task<Product> Add(Product product);
        Task<ICollection<Product>> GetAll();
        Task<ICollection<Product>> GetByCategoryID(Guid? ID);
        Task<Product> GetByID(Guid? ID);
        Task<Product> Update(Product product);
        Task Delete(Guid? ID);
    }
}
