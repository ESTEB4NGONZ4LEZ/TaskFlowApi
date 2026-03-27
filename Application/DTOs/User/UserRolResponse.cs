namespace Application.DTOs.User;

public sealed record UserRolResponse(int UserId, int RolId, DateTime AssignedAt);
