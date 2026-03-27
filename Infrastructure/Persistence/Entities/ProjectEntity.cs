namespace Infrastructure.Persistence.Entities;

public class ProjectEntity : IInactivatable
{
    public int ProjectId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }
    public bool IsActive { get; set; }

    public UserEntity CreatedByUser { get; set; }
    public List<TaskEntity> Tasks { get; set; }
}
