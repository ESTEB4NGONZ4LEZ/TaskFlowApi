using Application.Common;
using Domain.Ports.Repositories;
using FluentValidation;

namespace Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.UserName)
            .Required()
            .MaxLength(50)
            .MustAsync(async (userName, cancellation) => !await userRepository.ExistsWithUserNameAsync(userName))
            .WithMessage("A user with this username already exists.");

        RuleFor(x => x.Password)
            .Required()
            .MaxLength(255)
            .MinLength(8)
            .ValidPassword();

        RuleFor(x => x.Email)
            .Required()
            .MaxLength(100)
            .ValidEmail()
            .MustAsync(async (email, cancellation) => !await userRepository.ExistsWithEmailAsync(email))
            .WithMessage("A user with this email already exists.");
    }
}
