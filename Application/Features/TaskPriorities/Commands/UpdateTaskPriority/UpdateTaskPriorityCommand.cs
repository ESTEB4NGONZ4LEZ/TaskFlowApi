using Application.DTOs.TaskPriority;
using MediatR;

namespace Application.Features.TaskPriorities.Commands.UpdateTaskPriority;

public record UpdateTaskPriorityCommand(int TaskPriorityId, string Name, string? Description, int Level) : IRequest<TaskPriorityResponse>;
