using Microsoft.AspNetCore.Mvc;
using BookApi.Models;
using BookApi.DTOs;
using BookApi.Data; // Твой DbContext
using BCrypt.Net;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto request)
    {
        // 1. Проверяем, не занят ли Email
        if (_context.Users.Any(u => u.Email == request.Email))
        {
            return BadRequest("Пользователь с таким Email уже существует.");
        }

        // 2. Хэшируем пароль перед сохранением
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

       return Ok(new { 
        id = user.Id, 
        username = user.Username, 
        email = user.Email, 
        createdAt = user.CreatedAt // Добавили
    });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto request)
    {
        // 1. Ищем пользователя по Email
        var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
        
        // 2. Проверяем: существует ли он и совпадает ли пароль
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized("Неверный Email или пароль.");
        }

        // Возвращаем данные пользователя (в будущем тут можно добавить Token)
        return Ok(new { 
            id = user.Id, 
            username = user.Username, 
            email = user.Email, 
            createdAt = user.CreatedAt // Добавили
        });
    }
}