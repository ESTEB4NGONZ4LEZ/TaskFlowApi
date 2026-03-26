using Domain.Ports.Repositories;
using FluentValidation;

namespace Application.Features.Rol.Commands.CreateRol;

public class CreateRolCommandValidator : AbstractValidator<CreateRolCommand>
{
    public CreateRolCommandValidator(IRolRepository rolRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.")
            .MustAsync(async (name, cancellation) => !await rolRepository.ExistsWithNameAsync(name))
            .WithMessage("A role with this name already exists.");

        RuleFor(x => x.Description)
            .MaximumLength(150).WithMessage("Description must not exceed 150 characters.")
            .When(x => x.Description is not null);
    }
}
