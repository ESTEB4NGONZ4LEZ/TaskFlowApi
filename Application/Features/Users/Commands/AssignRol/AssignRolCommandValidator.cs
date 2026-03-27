using Domain.Ports.Repositories;
using FluentValidation;

namespace Application.Features.Users.Commands.AssignRol;

public class AssignRolCommandValidator : AbstractValidator<AssignRolCommand>
{
    public AssignRolCommandValidator(
        IUserRepository userRepository,
        IRolRepository rolRepository,
        IUserRolRepository userRolRepository)
    {
        RuleFor(x => x.UserId)
            .MustAsync(async (id, ct) => await userRepository.ExistsAsync(id))
            .WithMessage("User not found.");

        RuleFor(x => x.RolId)
            .MustAsync(async (id, ct) => await rolRepository.ExistsAsync(id))
            .WithMessage("Rol not found.")
            .MustAsync(async (command, rolId, ct) => !await userRolRepository.IsAssignedAsync(command.UserId, rolId))
            .WithMessage("This role is already assigned to the user.");
    }
}
