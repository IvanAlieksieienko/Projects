using Motopark.Core.Entities;
using Motopark.Core.IRepositories;
using Motopark.Core.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Motopark.Core.Services
{
    public class DeliveryService : IDeliveryService<Delivery>
    {
        private IDeliveryRepository<Delivery> _deliveryRepository;

        public DeliveryService(IDeliveryRepository<Delivery> deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task<Delivery> Add(Delivery item)
        {
            return await _deliveryRepository.Add(item);
        }

        public async Task Delete(Guid id)
        {
            await _deliveryRepository.Delete(id);
        }

        public async Task<ICollection<Delivery>> GetAll()
        {
            return await _deliveryRepository.GetAll();
        }

        public async Task<Delivery> GetByID(Guid id)
        {
            return await _deliveryRepository.GetByID(id);
        }

        public async Task<Delivery> GetByOrderID(Guid id)
        {
            return await _deliveryRepository.GetByOrderID(id);
        }

        public async Task<Delivery> Update(Delivery item)
        {
            return await _deliveryRepository.Update(item);
        }
    }
}
