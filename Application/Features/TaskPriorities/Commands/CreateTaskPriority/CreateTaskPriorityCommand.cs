using Application.DTOs.TaskPriority;
using MediatR;

namespace Application.Features.TaskPriorities.Commands.CreateTaskPriority;

public record CreateTaskPriorityCommand : IRequest<TaskPriorityResponse>
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public int Level { get; init; }
}
