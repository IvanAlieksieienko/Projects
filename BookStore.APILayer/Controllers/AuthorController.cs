using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.APILayer.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private IAuthorService _serviceAuthor;

        public AuthorController(IAuthorService authorService)
        {
            _serviceAuthor = authorService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var authors = _serviceAuthor.GetAll();
            if (authors != null)
                return Ok(authors);
            return NotFound();
            
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var author = _serviceAuthor.GetById(id);
            if (author != null)
                return Ok(author);
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var author = _serviceAuthor.GetById(id);
            if (author == null)
                return NotFound();
            _serviceAuthor.Delete(id);
            return Ok(author);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult AddAuthor([FromBody]AuthorInputModel fields)
        {
            _serviceAuthor.AddItem(fields);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateAuthor([FromBody]AuthorInputModel fields, int id)
        {
            _serviceAuthor.UpdateItem(id, fields);
            return Ok();
        }
    }
}