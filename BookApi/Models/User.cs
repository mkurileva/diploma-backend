namespace BookApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        
        // Это поле будет хранить зашифрованный пароль
        public string PasswordHash { get; set; } = string.Empty; 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Связь: один пользователь может иметь много заметок
        public List<Note> Notes { get; set; } = new();
    }
}