using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.APILayer.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private IBookService _serviceBook;
        private IAdminService _serviceAdmin;
        private IUserService _serviceUser;

        public BookController(IBookService bookService, IAdminService adminService, IUserService userService)
        {
            _serviceBook = bookService;
            _serviceUser = userService;
            _serviceAdmin = adminService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var books = _serviceBook.GetAll();
            if (books != null)
                return Ok(books);
            return NotFound(); 
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var book = _serviceBook.GetById(id);
            if (book != null)
                return Ok(book);
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var book = _serviceBook.GetById(id);
            if (book == null)
                return NotFound();
            _serviceBook.Delete(id);
            return Ok(book);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult AddBook([FromBody]BookInputModel fields)
        {
            _serviceBook.AddItem(fields);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateBook([FromBody]BookInputModel fields, int id)
        {
            _serviceBook.UpdateItem(id, fields);
            return Ok();
        }

        [Authorize(Roles = "user")]
        [HttpGet("Buy/BookID/{BookID}")]
        public IActionResult BuyBook(int BookID)
        {
            var currentUserEmail = HttpContext.User.Identity.Name;
            var currentUser = _serviceUser.GetByEmail(currentUserEmail);
            _serviceUser.BuyBook(currentUser.ID, BookID);
            _serviceUser.DeleteOrderedBook(currentUser.ID, BookID);
            return Ok();
        }


        [Authorize(Roles = "user")]
        [HttpGet("Order/BookID/{BookID}")]
        public IActionResult OrderBook(int BookID)
        {
            var currentUserEmail = HttpContext.User.Identity.Name;
            var currentUser = _serviceUser.GetByEmail(currentUserEmail);
            _serviceUser.OrderBook(currentUser.ID, BookID);
            return Ok();
        }

        [Authorize(Roles = "user")]
        [HttpDelete("Order/BookID/{BookID}")]
        public IActionResult DeleteOrderedBook(int BookID)
        {
            var currentUserEmail = HttpContext.User.Identity.Name;
            var currentUser = _serviceUser.GetByEmail(currentUserEmail);
            _serviceUser.DeleteOrderedBook(currentUser.ID, BookID);
            return Ok();
        }
    }
}
