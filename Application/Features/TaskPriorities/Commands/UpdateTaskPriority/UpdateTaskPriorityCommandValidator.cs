using Application.Common;
using Domain.Ports.Repositories;
using FluentValidation;

namespace Application.Features.TaskPriorities.Commands.UpdateTaskPriority;

public class UpdateTaskPriorityCommandValidator : AbstractValidator<UpdateTaskPriorityCommand>
{
    public UpdateTaskPriorityCommandValidator(ITaskPriorityRepository taskPriorityRepository)
    {
        RuleFor(x => x.TaskPriorityId)
            .MustAsync(async (id, ct) => await taskPriorityRepository.ExistsAsync(id))
            .WithMessage("Task priority not found.");

        RuleFor(x => x.Name)
            .Required()
            .MaxLength(50)
            .MustAsync(async (command, name, ct) => !await taskPriorityRepository.ExistsWithNameAsync(name, command.TaskPriorityId))
            .WithMessage("A task priority with this name already exists.");

        RuleFor(x => x.Description)
            .MaxLength(150)
            .When(x => x.Description is not null);

        RuleFor(x => x.Level)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Level must be greater than or equal to 1.")
            .MustAsync(async (command, level, ct) => !await taskPriorityRepository.ExistsWithLevelAsync(level, command.TaskPriorityId))
            .WithMessage("A task priority with this level already exists.");
    }
}
