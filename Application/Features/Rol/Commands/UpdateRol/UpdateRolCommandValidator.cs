using Application.Common;
using Domain.Ports.Repositories;
using FluentValidation;

namespace Application.Features.Rol.Commands.UpdateRol;

public class UpdateRolCommandValidator : AbstractValidator<UpdateRolCommand>
{
    public UpdateRolCommandValidator(IRolRepository rolRepository)
    {
        RuleFor(x => x.RolId)
            .MustAsync(async (id, cancellation) => await rolRepository.ExistsAsync(id))
            .WithMessage("Role not found.");

        RuleFor(x => x.Name)
            .Required()
            .MaxLength(50)
            .MustAsync(async (command, name, cancellation) => !await rolRepository.ExistsWithNameAsync(name, command.RolId))

            .WithMessage("A role with this name already exists.");

        RuleFor(x => x.Description)
            .MaxLength(150)
            .When(x => x.Description is not null);
    }
}
