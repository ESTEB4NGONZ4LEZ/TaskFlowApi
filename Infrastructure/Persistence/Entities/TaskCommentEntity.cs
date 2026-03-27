namespace Infrastructure.Persistence.Entities;

public class TaskCommentEntity
{
    public int TaskCommentId { get; set; }
    public string Description { get; set; }
    public int TaskId { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    public TaskEntity Task { get; set; }
    public UserEntity CreatedByUser { get; set; }
}
