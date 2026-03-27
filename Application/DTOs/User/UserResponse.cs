namespace Application.DTOs.User;

public sealed record UserResponse(int UserId, string UserName, string Email, bool IsActive, DateTime CreatedAt, DateTime? UpdatedAt);
