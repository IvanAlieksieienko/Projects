using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.APILayer.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _serviceUser;

        public UserController(IUserService userService)
        {
            _serviceUser = userService;
        }

        [Authorize(Roles = "user")]
        public IActionResult Index()
        {
            return Content(User.Identity.Name);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _serviceUser.GetAll();
            if (users != null)
                return Ok(users);
            return NotFound();
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _serviceUser.GetById(id);
            if (user != null)
                return Ok(user);
            return NotFound();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _serviceUser.GetById(id);
            if (user == null)
                return NotFound();
            _serviceUser.Delete(id);
            return Ok(user);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddUser([FromBody]UserInputModel fields)
        {
            _serviceUser.AddItem(fields);
            return Ok();
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateUser([FromBody]UserInputModel fields, int id)
        {
            bool isAdmin = HttpContext.User.IsInRole("admin");
            bool isUser = HttpContext.User.IsInRole("user");
            if (isAdmin)
                _serviceUser.UpdateItem(id, fields);
            if (isUser)
                _serviceUser.UpdateItem(_serviceUser.GetByEmail(HttpContext.User.Identity.Name).ID, fields);
            return Ok();
        }

        [Authorize(Roles = "user")]
        [HttpGet("{id:int}/Book")]
        public IActionResult GetUserBooks(int id)
        {
            var books = _serviceUser.GetUserBooks(id);
            if (books != null)
                return Ok(books);
            return NoContent();
        }

        [Authorize(Roles = "user")]
        [HttpGet("{id:int}/Basket")]
        public IActionResult GetUserBasket(int id)
        {
            var basket = _serviceUser.GetUserBasket(id);
            if (basket != null)
                return Ok(basket);
            return NoContent();
        }
    }
}