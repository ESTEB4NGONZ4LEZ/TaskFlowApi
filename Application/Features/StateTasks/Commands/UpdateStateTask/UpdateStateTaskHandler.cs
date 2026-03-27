using Application.DTOs.StateTask;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Ports;
using Domain.Ports.Repositories;
using MediatR;

namespace Application.Features.StateTasks.Commands.UpdateStateTask;

public class UpdateStateTaskHandler : IRequestHandler<UpdateStateTaskCommand, StateTaskResponse>
{
    private readonly IStateTaskRepository _stateTaskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStateTaskHandler(IStateTaskRepository stateTaskRepository, IUnitOfWork unitOfWork)
    {
        _stateTaskRepository = stateTaskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<StateTaskResponse> Handle(UpdateStateTaskCommand command, CancellationToken cancellationToken)
    {
        var stateTask = await _stateTaskRepository.GetByIdAsync(command.StateTaskId)
            ?? throw new NotFoundException(nameof(StateTask), command.StateTaskId);

        stateTask.Update(command.Name, command.Description);

        await _stateTaskRepository.UpdateAsync(stateTask);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new StateTaskResponse(stateTask.StateTaskId, stateTask.Name, stateTask.Description);
    }
}
