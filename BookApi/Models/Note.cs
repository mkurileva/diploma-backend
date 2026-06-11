namespace BookApi.Models
{
    public class Note
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public int UserId { get; set; }

        public int ParagraphIndex { get; set; }

        public int Start { get; set; }
        public int End { get; set; }

        public string Text { get; set; } = string.Empty;
        public string? NoteText { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Color { get; set; } = "yellow";
        public User? User { get; set; }  


    }
}