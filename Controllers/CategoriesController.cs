using BookStoreAPI.Model.Data;
using BookStoreAPI.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly BookDbContext db;
        public CategoriesController(BookDbContext _db)
        {
            db = _db;
        }
        //[HttpGet]

        //public IActionResult GetCategories()
        //{
        //    var categories = db.Categories.ToList();
        //    return Ok(categories);
        //}
        [HttpPost]
        [Consumes("application/json")]
        public IActionResult Add(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Categories.Add(category);
            db.SaveChanges();
            return Created($"/api/Categories/{category.Id}", category);
        }
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Book>(StatusCodes.Status200OK)]
        public IActionResult Put([FromQuery] int id, [FromBody] Category category)
        {
            var c = db.Categories.Find(id);
            if (c == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            c.Name = category.Name;
            db.SaveChanges();
            return Ok();

        }
        [HttpDelete]
        [Consumes("application/json")]
        public IActionResult Delete(int id)
        {
            var c = db.Categories.Find(id);
            if (c == null)
            {
                return NotFound();
            }
            db.Categories.Remove(c);
            db.SaveChanges();
            return Ok();
        }
    }
}
