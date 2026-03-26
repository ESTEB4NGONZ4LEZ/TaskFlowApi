namespace Infrastructure.Persistence.Entities;

public class StateTask
{
    public int StateTaskId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public List<Task> Tasks { get; set; }
    public List<RecordStateTask> RecordStateTasks { get; set; }
}