using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.APILayer.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IAdminService _service;

        public AdminController(IAdminService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return Content(User.Identity.Name);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var admins = _service.GetAll();
            if (admins != null)
                return Ok(admins);
            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var admin = _service.GetById(id);
            if (admin != null)
                return Ok(admin);
            return NotFound();
        }

        [HttpGet("GetCurrentAdmin")]
        public IActionResult GetCurrentAdmin()
        {
            var admin = _service.GetByEmail(HttpContext.User.Identity.Name);
            if (admin != null)
                return Ok(admin);
            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var admin = _service.GetById(id);
            if (admin == null)
                return NotFound();
            _service.Delete(id);
            return Ok(admin);
        }

        [HttpPost]
        public IActionResult AddAdmin([FromBody]AdminInputModel fields)
        {
            _service.AddItem(fields);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAdmin([FromBody]AdminInputModel fields, int id)
        {
            _service.UpdateItem(id, fields);
            return Ok();
        }
    }
}