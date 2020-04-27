using BookStore.BusinessLogicLayer.InputModels;
using BookStore.BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.APILayer.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private IGenreService _serviceGenre;

        public GenreController(IGenreService genreService)
        {
            _serviceGenre = genreService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var genres = _serviceGenre.GetAll();
            if (genres != null)
                return Ok(genres);
            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var genre = _serviceGenre.GetById(id);
            if (genre != null)
                return Ok(genre);
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var genre = _serviceGenre.GetById(id);
            if (genre == null)
                return NotFound();
            _serviceGenre.Delete(id);
            return Ok(genre);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult AddGenre([FromBody]GenreInputModel fields)
        {
            _serviceGenre.AddItem(fields);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateGenre([FromBody]GenreInputModel fields, int id)
        {
            _serviceGenre.UpdateItem(id, fields);
            return Ok();
        }
    }
}