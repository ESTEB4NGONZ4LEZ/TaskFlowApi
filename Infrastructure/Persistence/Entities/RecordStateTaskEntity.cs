namespace Infrastructure.Persistence.Entities;

public class RecordStateTaskEntity
{
    public int RecordStateTaskId { get; set; }
    public int TaskId { get; set; }
    public int StateTaskId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ChangedBy { get; set; }

    public TaskEntity Task { get; set; }
    public StateTaskEntity StateTask { get; set; }
    public UserEntity ChangedByUser { get; set; }
}
