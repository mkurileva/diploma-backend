namespace BookApi.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public bool IsBuiltIn { get; set; } = false;

        // Добавляем связь с пользователем
        public int? UserId { get; set; } 
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}