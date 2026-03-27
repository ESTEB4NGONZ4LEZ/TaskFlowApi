using Application.DTOs.StateTask;
using MediatR;

namespace Application.Features.StateTasks.Commands.CreateStateTask;

public record CreateStateTaskCommand : IRequest<StateTaskResponse>
{
    public string Name { get; init; }
    public string? Description { get; init; }
}
