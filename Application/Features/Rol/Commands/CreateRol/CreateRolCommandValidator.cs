using Application.Common;
using Domain.Ports.Repositories;
using FluentValidation;

namespace Application.Features.Rol.Commands.CreateRol;

public class CreateRolCommandValidator : AbstractValidator<CreateRolCommand>
{
    public CreateRolCommandValidator(IRolRepository rolRepository)
    {
        RuleFor(x => x.Name)
            .Required()
            .MaxLength(50)
            .MustAsync(async (name, cancellation) => !await rolRepository.ExistsWithNameAsync(name))
            .WithMessage("A role with this name already exists.");

        RuleFor(x => x.Description)
            .MaxLength(150)
            .When(x => x.Description is not null);
    }
}
