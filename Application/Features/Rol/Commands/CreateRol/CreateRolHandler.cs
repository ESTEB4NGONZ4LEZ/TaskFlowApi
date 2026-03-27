using Application.DTOs.Rol;
using Domain.Entities;
using Domain.Ports;
using Domain.Ports.Repositories;
using MediatR;

namespace Application.Features.Roles.Commands.CreateRol;

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
        var rol = Rol.Create(command.Name, command.Description);

        var getCreated = await _rolRepository.CreateAsync(rol);
        await _unitOfWork.CommitAsync(cancellationToken);
        var created = getCreated();

        return new RolResponse(created.RolId, created.Name, created.Description, created.CreatedAt);
    }
}
