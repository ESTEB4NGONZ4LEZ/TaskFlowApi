namespace Infrastructure.Persistence.Entities;

public class UserEntity : IInactivatable
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<UserRolEntity> UserRols { get; set; }
    public List<ProjectEntity> Projects { get; set; }
    public List<TaskEntity> CreatedTasks { get; set; }
    public List<TaskEntity> AssignedTasks { get; set; }
    public List<RecordStateTaskEntity> RecordStateTasks { get; set; }
    public List<TaskCommentEntity> TaskComments { get; set; }
    public List<UserTaskEntity> UserTasks { get; set; }
}
