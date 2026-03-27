using Application.DTOs.User;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<UserResponse>
{
    public string UserName { get; init; }
    public string Password { get; init; }
    public string Email { get; init; }
}
