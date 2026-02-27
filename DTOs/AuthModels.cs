namespace Security.DTOs;

public record RegisterDto(string Email, string Password, string Role);
public record LoginDto(string Email, string Password);
public record AuthResponse(string Token);
