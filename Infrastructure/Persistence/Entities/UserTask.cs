namespace Infrastructure.Persistence.Entities;

public class UserTask
{
    public int TaskId { get; set; }
    public int UserId { get; set; }
    public DateTime AssignedAt { get; set; }

    public Task Task { get; set; }
    public User User { get; set; }
}
