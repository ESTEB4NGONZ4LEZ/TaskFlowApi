namespace Infrastructure.Persistence.Entities;

public class TaskEntity
{
    public int TaskId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ProjectId { get; set; }
    public int StateTaskId { get; set; }
    public int TaskPriorityId { get; set; }
    public int CreatedBy { get; set; }
    public int AssignedTo { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ProjectEntity Project { get; set; }
    public StateTaskEntity StateTask { get; set; }
    public TaskPriorityEntity TaskPriority { get; set; }
    public UserEntity CreatedByUser { get; set; }
    public UserEntity AssignedToUser { get; set; }

    public List<RecordStateTaskEntity> RecordStateTasks { get; set; }
    public List<TaskCommentEntity> TaskComments { get; set; }
    public List<UserTaskEntity> UserTasks { get; set; }
}
