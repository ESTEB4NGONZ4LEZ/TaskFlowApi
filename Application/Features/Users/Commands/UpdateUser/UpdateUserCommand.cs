using Application.DTOs.User;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(int UserId, string UserName, string Email, bool IsActive, string? Password) : IRequest<UserResponse>;
