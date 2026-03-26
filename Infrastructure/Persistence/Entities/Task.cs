namespace Infrastructure.Persistence.Entities;

public class Task
{
    public int TaskId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ProjectId { get; set; }
    public int StateTaskId { get; set; }
    public int TaskPriorityId { get; set; }

    public Project Project { get; set; }
    public StateTask StateTask { get; set; }
    public TaskPriority TaskPriority { get; set; }

    public List<RecordStateTask> RecordStateTasks { get; set; }
    public List<TaskComment> TaskComments { get; set; }
    public List<UserTask> UserTasks { get; set; }
}
