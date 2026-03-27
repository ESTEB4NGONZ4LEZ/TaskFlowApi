using Application.Common;
using Domain.Ports.Repositories;
using FluentValidation;

namespace Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.UserId)
            .MustAsync(async (id, cancellation) => await userRepository.ExistsAsync(id))
            .WithMessage("User not found.");

        RuleFor(x => x.UserName)
            .Required()
            .MaxLength(50)
            .MustAsync(async (command, userName, cancellation) => !await userRepository.ExistsWithUserNameAsync(userName, command.UserId))
            .WithMessage("A user with this username already exists.");

        RuleFor(x => x.Email)
            .Required()
            .MaxLength(100)
            .ValidEmail()
            .MustAsync(async (command, email, cancellation) => !await userRepository.ExistsWithEmailAsync(email, command.UserId))
            .WithMessage("A user with this email already exists.");

        RuleFor(x => x.Password)
            .MaxLength(255)
            .MinLength(8)
            .ValidPassword()
            .When(x => x.Password is not null);
    }
}
