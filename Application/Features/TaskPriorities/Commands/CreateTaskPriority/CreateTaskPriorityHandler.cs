using Application.DTOs.TaskPriority;
using Domain.Entities;
using Domain.Ports;
using Domain.Ports.Repositories;
using MediatR;

namespace Application.Features.TaskPriorities.Commands.CreateTaskPriority;

public class CreateTaskPriorityHandler : IRequestHandler<CreateTaskPriorityCommand, TaskPriorityResponse>
{
    private readonly ITaskPriorityRepository _taskPriorityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaskPriorityHandler(ITaskPriorityRepository taskPriorityRepository, IUnitOfWork unitOfWork)
    {
        _taskPriorityRepository = taskPriorityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TaskPriorityResponse> Handle(CreateTaskPriorityCommand command, CancellationToken cancellationToken)
    {
        var taskPriority = TaskPriority.Create(command.Name, command.Description, command.Level);

        var getCreated = await _taskPriorityRepository.CreateAsync(taskPriority);
        await _unitOfWork.CommitAsync(cancellationToken);
        var created = getCreated();

        return new TaskPriorityResponse(created.TaskPriorityId, created.Name, created.Description, created.Level);
    }
}
