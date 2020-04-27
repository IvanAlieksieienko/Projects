using Motopark.Core.Entities;
using Motopark.Core.IRepositories;
using Motopark.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motopark.Core.Services
{
    public class BasketService : IBasketService<Basket>
    {
        private IBasketRepository<Basket> _basketRepository;

        public BasketService(IBasketRepository<Basket> basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<Basket> Add(Basket item)
        {
            if (item.ID == null || item.ID.ToString() == "" || item.ID == Guid.Empty) item.ID = Guid.NewGuid();
            var baskets = await GetByBasketID(item.ID);
            Basket basket;
            if (baskets != null)
            {
                basket = baskets.FirstOrDefault(p => p.ProductID == item.ProductID);
                if (basket != null)
                {
                    return await ChangeCount(item.ID, item.ProductID, ++basket.Count);
                }
                basket = await _basketRepository.Add(item);
            }
            else
            {
                basket = await _basketRepository.Add(item);
            }

            return basket;
        }

        public async Task<Basket> ChangeCount(Guid basketId, Guid productId, int count)
        {
            return await _basketRepository.ChangeCount(basketId, productId, count);
        }

        public async Task Delete(Guid id)
        {
            await _basketRepository.Delete(id);
        }

        public async Task DeleteByProduct(Guid id, Guid basketId)
        {
            await _basketRepository.DeleteByProduct(id, basketId);
        }

        public async Task<ICollection<Basket>> GetAll()
        {
            return await _basketRepository.GetAll();
        }

        public async Task<ICollection<Basket>> GetByBasketID(Guid id)
        {
            return await _basketRepository.GetByBasketID(id);
        }
    }
}
