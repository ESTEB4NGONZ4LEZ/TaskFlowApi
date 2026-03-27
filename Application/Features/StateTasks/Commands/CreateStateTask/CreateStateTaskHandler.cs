using Application.DTOs.StateTask;
using Domain.Entities;
using Domain.Ports;
using Domain.Ports.Repositories;
using MediatR;

namespace Application.Features.StateTasks.Commands.CreateStateTask;

public class CreateStateTaskHandler : IRequestHandler<CreateStateTaskCommand, StateTaskResponse>
{
    private readonly IStateTaskRepository _stateTaskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateStateTaskHandler(IStateTaskRepository stateTaskRepository, IUnitOfWork unitOfWork)
    {
        _stateTaskRepository = stateTaskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<StateTaskResponse> Handle(CreateStateTaskCommand command, CancellationToken cancellationToken)
    {
        var stateTask = StateTask.Create(command.Name, command.Description);

        var getCreated = await _stateTaskRepository.CreateAsync(stateTask);
        await _unitOfWork.CommitAsync(cancellationToken);
        var created = getCreated();

        return new StateTaskResponse(created.StateTaskId, created.Name, created.Description);
    }
}
