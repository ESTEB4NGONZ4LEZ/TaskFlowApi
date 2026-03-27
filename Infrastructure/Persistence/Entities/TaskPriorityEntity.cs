namespace Infrastructure.Persistence.Entities;

public class TaskPriorityEntity
{
    public int TaskPriorityId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Level { get; set; }

    public List<TaskEntity> Tasks { get; set; }
}
