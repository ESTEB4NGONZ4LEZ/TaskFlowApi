namespace Infrastructure.Persistence.Entities;

public class StateTaskEntity
{
    public int StateTaskId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public List<TaskEntity> Tasks { get; set; }
    public List<RecordStateTaskEntity> RecordStateTasks { get; set; }
}
