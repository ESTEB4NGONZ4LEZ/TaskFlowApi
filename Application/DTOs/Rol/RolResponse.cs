namespace Application.DTOs.Rol;

public sealed record RolResponse(int RolId, string Name, string? Description, DateTime CreatedAt);
