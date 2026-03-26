using Application.DTOs.Rol;
using Domain.Ports;
using Domain.Ports.Repositories;
using MediatR;
using RolEntity = Domain.Entities.Rol;

namespace Application.Features.Rol.Commands.CreateRol;

public class CreateRolHandler : IRequestHandler<CreateRolCommand, RolResponse>
{
    private readonly IRolRepository _rolRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRolHandler(IRolRepository rolRepository, IUnitOfWork unitOfWork)
    {
        _rolRepository = rolRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RolResponse> Handle(CreateRolCommand command, CancellationToken cancellationToken)
    {
        var rol = RolEntity.Create(command.Name, command.Description);

        var getCreated = await _rolRepository.CreateAsync(rol);
        await _unitOfWork.CommitAsync(cancellationToken);
        var created = getCreated();

        return new RolResponse
        {
            RolId = created.RolId,
            Name = created.Name,
            Description = created.Description,
            CreatedAt = created.CreatedAt
        };
    }
}
