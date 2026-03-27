using Application.DTOs.TaskPriority;
using Domain.Exceptions;
using Domain.Ports;
using Domain.Ports.Repositories;
using MediatR;

namespace Application.Features.TaskPriorities.Commands.UpdateTaskPriority;

public class UpdateTaskPriorityHandler : IRequestHandler<UpdateTaskPriorityCommand, TaskPriorityResponse>
{
    private readonly ITaskPriorityRepository _taskPriorityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTaskPriorityHandler(ITaskPriorityRepository taskPriorityRepository, IUnitOfWork unitOfWork)
    {
        _taskPriorityRepository = taskPriorityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TaskPriorityResponse> Handle(UpdateTaskPriorityCommand command, CancellationToken cancellationToken)
    {
        var taskPriority = await _taskPriorityRepository.GetByIdAsync(command.TaskPriorityId)
            ?? throw new NotFoundException("TaskPriority", command.TaskPriorityId);

        taskPriority.Update(command.Name, command.Description, command.Level);

        await _taskPriorityRepository.UpdateAsync(taskPriority);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new TaskPriorityResponse(taskPriority.TaskPriorityId, taskPriority.Name, taskPriority.Description, taskPriority.Level);
    }
}
