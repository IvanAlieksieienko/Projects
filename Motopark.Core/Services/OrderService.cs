using Motopark.Core.Entities;
using Motopark.Core.IRepositories;
using Motopark.Core.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Motopark.Core.Services
{
    public class OrderService : IOrderService<Order>
    {
        private IOrderRepository<Order> _orderRepository;

        public OrderService(IOrderRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> Add(Order item)
        {
            return await _orderRepository.Add(item);
        }

        public async Task Delete(Guid id)
        {
            await _orderRepository.Delete(id);
        }

        public async Task<ICollection<Order>> GetAll()
        {
            return await _orderRepository.GetAll();
        }

        public async Task<Order> GetByID(Guid id)
        {
            return await _orderRepository.GetByID(id);
        }

        public async Task<Order> Update(Order item)
        {
            return await _orderRepository.Update(item);
        }
    }
}
