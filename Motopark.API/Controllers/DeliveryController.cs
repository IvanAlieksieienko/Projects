using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Motopark.Core.Entities;
using Motopark.Core.IServices;

namespace Motopark.API.Controllers
{
    [Route("delivery")]
    [ApiController]
    public class DeliveryController : Controller
    {
        private IDeliveryService<Delivery> _deliveryService;
        
        public DeliveryController(IDeliveryService<Delivery> deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByOrderID(Guid id)
        {
            var delivery = await _deliveryService.GetByOrderID(id);
            return Ok(delivery);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(Delivery item)
        {
            var delivery = await _deliveryService.Add(item);
            return Ok(delivery);
        }
    }
}