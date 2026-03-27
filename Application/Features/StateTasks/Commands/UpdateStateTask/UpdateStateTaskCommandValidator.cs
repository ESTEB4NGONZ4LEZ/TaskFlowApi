using Application.Common;
using Domain.Ports.Repositories;
using FluentValidation;

namespace Application.Features.StateTasks.Commands.UpdateStateTask;

public class UpdateStateTaskCommandValidator : AbstractValidator<UpdateStateTaskCommand>
{
    public UpdateStateTaskCommandValidator(IStateTaskRepository stateTaskRepository)
    {
        RuleFor(x => x.StateTaskId)
            .MustAsync(async (id, cancellation) => await stateTaskRepository.ExistsAsync(id))
            .WithMessage("State task not found.");

        RuleFor(x => x.Name)
            .Required()
            .MaxLength(50)
            .MustAsync(async (command, name, cancellation) => !await stateTaskRepository.ExistsWithNameAsync(name, command.StateTaskId))
            .WithMessage("A state task with this name already exists.");

        RuleFor(x => x.Description)
            .MaxLength(150)
            .When(x => x.Description is not null);
    }
}
