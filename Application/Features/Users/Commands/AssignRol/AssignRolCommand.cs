using Application.DTOs.User;
using MediatR;

namespace Application.Features.Users.Commands.AssignRol;

public record AssignRolCommand(int UserId, int RolId) : IRequest<UserRolResponse>;
