namespace Infrastructure.Persistence.Entities;

public class TaskComment
{
    public int TaskCommentId { get; set; }
    public string Description { get; set; }
    public int TaskId { get; set; }

    public Task Task { get; set; }
}
