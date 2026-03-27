namespace Domain.Entities;

public class UserRol
{
    public int UserId { get; private set; }
    public int RolId { get; private set; }
    public DateTime AssignedAt { get; private set; }

    private UserRol() { }

    public static UserRol Create(int userId, int rolId) =>
        new() { UserId = userId, RolId = rolId, AssignedAt = DateTime.UtcNow };

    public static UserRol Reconstitute(int userId, int rolId, DateTime assignedAt) =>
        new() { UserId = userId, RolId = rolId, AssignedAt = assignedAt };
}
