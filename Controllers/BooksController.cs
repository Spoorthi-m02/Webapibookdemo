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
    public class BooksController : ControllerBase
    {
        private readonly BookDbContext db;
        public BooksController(BookDbContext _db)
        {
            db = _db;
        }
        #region
        //[HttpGet]
        //public IActionResult GetBooks()
        //{
        //    var books = db.Books.ToList();
        //    return Ok(books);
        //}
        //[HttpGet]
        //[Route("{id:int}")]   // placehlder({id}) should be same as parameter name
        //public IActionResult Get(int id)
        //{
        //    var product = db.Books.Find(id);
        //    if (product == null)
        //    {
        //        // return 404 - not found
        //        return NotFound();
        //    }
        //    else
        //    {
        //        return Ok(product);
        //    }
        //}
        //[HttpGet]
        //[Route("{title:alpha}")]   // placehlder({id}) should be same as parameter name
        //public IActionResult Get1(string title)
        //{
        //    var product = from b in db.Books
        //                  where b.Title == title
        //                  select b;
        //    if (product == null)
        //    {
        //        // return 404 - not found
        //        return NotFound();
        //    }
        //    else
        //    {
        //        return Ok(product);
        //    }
        //}
        #endregion
        [HttpGet]
        [Route("{id:int}")]   // placehlder({id}) should be same as parameter name
        [Produces("application/json")]
        public IActionResult Get(int id)
        {
            var product = db.Books.Find(id);
            if (product == null)
            {
                // return 404 - not found
                return NotFound();
            }
            else
            {

                // return 200 - ok with data
                return Ok(product);
            }
        }
        [HttpPost]
        [Consumes("application/json")]
        public IActionResult Add(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Books.Add(book);
            db.SaveChanges();
            return Created($"/api/Books/{book.Id}", book);
        }
        [HttpDelete]
        [Consumes("application/json")]
        [ProducesResponseType<Book>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var product = db.Books.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            db.Books.Remove(product);
            db.SaveChanges();
            return Ok();
        }
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Book>(StatusCodes.Status200OK)]
        public IActionResult Put([FromQuery] int id, [FromBody] Book book)
        {
            var b = db.Books.Find(id);
            if (b == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            b.Title = book.Title;
            b.ISBN = book.ISBN;
            b.AuthorId = book.AuthorId;
            b.CategoryId = book.CategoryId;
            b.PublishedDate = book.PublishedDate;
            db.SaveChanges();
            return Ok();

        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProduct(int id, [FromBody] JsonPatchDocument<Book> patchbDoc)
        {
            if (patchbDoc == null)
            {
                return BadRequest();
            }

            var book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            patchbDoc.ApplyTo(book, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await db.SaveChangesAsync();

            return Ok(book);
        }
    }
}

