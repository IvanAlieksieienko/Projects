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
    [Route("order")]
    [ApiController]
    public class OrderController : Controller
    {
        private IOrderService<Order> _deliveryService;

        public OrderController(IOrderService<Order> deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Order item)
        {
            item.CreationTime = DateTime.Now;
            var order = await _deliveryService.Add(item);
            return Ok(order);
        }
    }
}