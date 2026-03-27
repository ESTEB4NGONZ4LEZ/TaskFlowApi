namespace Application.DTOs.TaskPriority;

public sealed record TaskPriorityResponse(int TaskPriorityId, string Name, string? Description, int Level);
