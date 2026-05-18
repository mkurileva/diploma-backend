using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookApi.Data;
using BookApi.Models;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotesController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Получить заметки конкретной книги для конкретного пользователя
        [HttpGet("book/{bookId}")]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes(int bookId, [FromQuery] int? userId)
        {
            if (userId == null) return Ok(new List<Note>());

            // Убедись, что здесь стоит && n.UserId == userId
            var notes = await _context.Notes
                .Where(n => n.BookId == bookId && n.UserId == userId) 
                .OrderBy(n => n.Id)
                .ToListAsync();

            return Ok(notes);
        }

        // 2. Создать заметку (теперь userId должен приходить в теле запроса Note)
        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote(Note note)
        {
            // Проверяем, указан ли автор заметки
            if (note.UserId <= 0)
                return BadRequest("Не указан идентификатор пользователя.");

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetNotes),
                new { bookId = note.BookId, userId = note.UserId },
                note
            );
        }

        //метод для получения всех заметок пользователя (чтобы посчитать их количество)
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Note>>> GetUserNotes(int userId)
        {
            return await _context.Notes.Where(n => n.UserId == userId).ToListAsync();
        }

        // 3. Удалить заметку (добавляем проверку userId из query, чтобы нельзя было удалить чужую)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id, [FromQuery] int userId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n =>
                    n.Id == id &&
                    n.UserId == userId);

            if (note == null)
                return NotFound("Заметка не найдена или у вас нет прав на её удаление.");

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 4. Обновить заметку
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] Note updated, [FromQuery] int userId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n =>
                    n.Id == id &&
                    n.UserId == userId);

            if (note == null)
                return NotFound("Заметка не найдена или у вас нет прав на её редактирование.");

            note.NoteText = updated.NoteText;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}