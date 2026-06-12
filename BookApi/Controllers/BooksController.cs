using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookApi.Data;
using BookApi.Models;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks([FromQuery] int? userId, [FromQuery] bool all = false)
    {
        var query = _context.Books.AsQueryable();

        if (all)
        {
            return await query.ToListAsync();
        }

        if (userId != null)
        {
            // Покажи все книги, где UserId совпадает, на всякий случай уберем !b.IsBuiltIn для теста счетчика
            query = query.Where(b => b.UserId == userId); 
        }
        else
        {
            // Если userId не указан (например, на главной), отдаем только встроенные
            query = query.Where(b => b.IsBuiltIn);
        }

        return await query.ToListAsync();
    }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            book.CreatedAt = DateTime.UtcNow;
            book.UpdatedAt = DateTime.UtcNow;

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book updatedBook)
        {
            if (id != updatedBook.Id)
                return BadRequest();

            var existingBook = await _context.Books.FindAsync(id);

            if (existingBook == null)
                return NotFound();

            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.Text = updatedBook.Text;
            existingBook.IsBuiltIn = updatedBook.IsBuiltIn;
            existingBook.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
                return NotFound();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}