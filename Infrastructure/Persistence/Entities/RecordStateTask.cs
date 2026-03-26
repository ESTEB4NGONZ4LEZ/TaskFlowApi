namespace Infrastructure.Persistence.Entities;

public class RecordStateTask
{
    public int RecordStateTaskId { get; set; }
    public int TaskId { get; set; }
    public int StateTaskId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Task Task { get; set; }
    public StateTask StateTask { get; set; }
}
