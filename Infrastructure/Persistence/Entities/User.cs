namespace Infrastructure.Persistence.Entities;

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<UserRol> UserRols { get; set; }
    public List<Project> Projects { get; set; }
    public List<Task> CreatedTasks { get; set; }
    public List<Task> AssignedTasks { get; set; }
    public List<RecordStateTask> RecordStateTasks { get; set; }
    public List<TaskComment> TaskComments { get; set; }
    public List<UserTask> UserTasks { get; set; }
}
