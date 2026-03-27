namespace Infrastructure.Persistence.Entities;

public class UserTaskEntity
{
    public int TaskId { get; set; }
    public int UserId { get; set; }
    public DateTime AssignedAt { get; set; }

    public TaskEntity Task { get; set; }
    public UserEntity User { get; set; }
}
