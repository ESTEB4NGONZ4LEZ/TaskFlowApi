using Application.DTOs.Rol;
using Domain.Ports.Repositories;
using MediatR;
using RolEntity = Domain.Entities.Rol;

namespace Application.Features.Rol.Commands.CreateRol;

public class CreateRolHandler : IRequestHandler<CreateRolCommand, RolResponse>
{
    private readonly IRolRepository _rolRepository;

    public CreateRolHandler(IRolRepository rolRepository)
    {
        _rolRepository = rolRepository;
    }

    public async Task<RolResponse> Handle(CreateRolCommand command, CancellationToken cancellationToken)
    {
        var rol = RolEntity.Create(command.Name, command.Description);
        var created = await _rolRepository.CreateAsync(rol);

        return new RolResponse
        {
            RolId = created.RolId,
            Name = created.Name,
            Description = created.Description,
            CreatedAt = created.CreatedAt
        };
    }
}
