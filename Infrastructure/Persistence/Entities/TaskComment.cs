namespace Infrastructure.Persistence.Entities;

public class TaskComment
{
    public int TaskCommentId { get; set; }
    public string Description { get; set; }
    public int TaskId { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    public Task Task { get; set; }
    public User CreatedByUser { get; set; }
}
