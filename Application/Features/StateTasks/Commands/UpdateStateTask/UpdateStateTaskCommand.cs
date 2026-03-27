using Application.DTOs.StateTask;
using MediatR;

namespace Application.Features.StateTasks.Commands.UpdateStateTask;

public record UpdateStateTaskCommand(int StateTaskId, string Name, string? Description) : IRequest<StateTaskResponse>;
