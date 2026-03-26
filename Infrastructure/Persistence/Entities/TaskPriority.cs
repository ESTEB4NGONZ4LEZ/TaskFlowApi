namespace Infrastructure.Persistence.Entities;

public class TaskPriority
{
    public int TaskPriorityId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public List<Task> Tasks { get; set; }
}
