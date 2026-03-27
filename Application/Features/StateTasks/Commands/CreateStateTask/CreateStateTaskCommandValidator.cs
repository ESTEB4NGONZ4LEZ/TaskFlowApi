using Application.Common;
using Domain.Ports.Repositories;
using FluentValidation;

namespace Application.Features.StateTasks.Commands.CreateStateTask;

public class CreateStateTaskCommandValidator : AbstractValidator<CreateStateTaskCommand>
{
    public CreateStateTaskCommandValidator(IStateTaskRepository stateTaskRepository)
    {
        RuleFor(x => x.Name)
            .Required()
            .MaxLength(50)
            .MustAsync(async (name, cancellation) => !await stateTaskRepository.ExistsWithNameAsync(name))
            .WithMessage("A state task with this name already exists.");

        RuleFor(x => x.Description)
            .MaxLength(150)
            .When(x => x.Description is not null);
    }
}
