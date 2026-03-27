namespace API.DTOs.TaskPriority;

public record UpdateTaskPriorityRequest(string Name, string? Description, int Level);
