using Application.Common;
using Domain.Ports.Repositories;
using FluentValidation;

namespace Application.Features.TaskPriorities.Commands.CreateTaskPriority;

public class CreateTaskPriorityCommandValidator : AbstractValidator<CreateTaskPriorityCommand>
{
    public CreateTaskPriorityCommandValidator(ITaskPriorityRepository taskPriorityRepository)
    {
        RuleFor(x => x.Name)
            .Required()
            .MaxLength(50)
            .MustAsync(async (name, ct) => !await taskPriorityRepository.ExistsWithNameAsync(name))
            .WithMessage("A task priority with this name already exists.");

        RuleFor(x => x.Description)
            .MaxLength(150)
            .When(x => x.Description is not null);

        RuleFor(x => x.Level)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Level must be greater than or equal to 1.")
            .MustAsync(async (level, ct) => !await taskPriorityRepository.ExistsWithLevelAsync(level))
            .WithMessage("A task priority with this level already exists.");
    }
}
