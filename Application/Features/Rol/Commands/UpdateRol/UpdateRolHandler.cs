using Application.DTOs.Rol;
using Domain.Exceptions;
using Domain.Ports;
using Domain.Ports.Repositories;
using MediatR;

namespace Application.Features.Roles.Commands.UpdateRol;

public class UpdateRolHandler : IRequestHandler<UpdateRolCommand, RolResponse>
{
    private readonly IRolRepository _rolRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRolHandler(IRolRepository rolRepository, IUnitOfWork unitOfWork)
    {
        _rolRepository = rolRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RolResponse> Handle(UpdateRolCommand command, CancellationToken cancellationToken)
    {
        var rol = await _rolRepository.GetByIdAsync(command.RolId)
            ?? throw new NotFoundException("Rol", command.RolId);

        rol.Update(command.Name, command.Description);

        await _rolRepository.UpdateAsync(rol);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new RolResponse(rol.RolId, rol.Name, rol.Description, rol.CreatedAt);
    }
}
