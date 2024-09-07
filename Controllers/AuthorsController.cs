using BookStoreAPI.Model.Data;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Model.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookDbContext db;
        public AuthorsController(BookDbContext _db)
        {
            db = _db;
        }
        //[HttpGet]
        //public IActionResult GetAuthors()
        //{
        //    var authors = db.Authors.ToList();
        //    return Ok(authors);
        //}

        [HttpGet]
        [Route("{id:int}")]   // placehlder({id}) should be same as parameter name
        [Produces("application/json")]
        public IActionResult Get(int id)
        {
            var auth = db.Authors.Find(id);
            if (auth == null)
            {
                // return 404 - not found
                return NotFound();
            }
            else
            {

                // return 200 - ok with data
                return Ok(auth);
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult Add(Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Authors.Add(author);
            db.SaveChanges();
            return Created($"/api/Authors{author.Id}",author);
        }
        [HttpPut]
        public IActionResult Put([FromQuery] int id, [FromBody] Author author)
        {
            var a = db.Authors.Find(id);
            if (a == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            a.Name = author.Name;
            a.Biography = author.Biography;
            db.SaveChanges();
            return Ok();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProduct(int id, [FromBody] JsonPatchDocument<Author> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var product = await db.Authors.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(product, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await db.SaveChangesAsync();

            return Ok(product);
        }


    }
}

