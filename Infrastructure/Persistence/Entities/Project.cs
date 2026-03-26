namespace Infrastructure.Persistence.Entities;

public class Project
{
    public int ProjectId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }
    public bool IsActive { get; set; }

    public User CreatedByUser { get; set; }
    public List<Task> Tasks { get; set; }
}