using Application.DTOs.Rol;
using MediatR;

namespace Application.Features.Roles.Commands.UpdateRol;

public record UpdateRolCommand(int RolId, string Name, string? Description) : IRequest<RolResponse>;
