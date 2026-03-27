namespace API.DTOs.User;

public record UpdateUserRequest(string UserName, string Email, bool IsActive, string? Password);
