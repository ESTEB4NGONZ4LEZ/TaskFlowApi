using Application.DTOs.Rol;
using MediatR;

namespace Application.Features.Rol.Commands.CreateRol;

public record CreateRolCommand : IRequest<RolResponse>
{
    public string Name { get; init; }
    public string Description { get; init; }
}
