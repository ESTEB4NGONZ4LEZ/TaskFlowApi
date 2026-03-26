using Application.DTOs.Rol;
using Domain.Ports.Repositories;
using RolModel = Domain.Entities.Rol;

namespace Application.UseCases.Rol;

public class CreateRolUseCase
{
    private readonly IRolRepository _rolRepository;

    public CreateRolUseCase(IRolRepository rolRepository)
    {
        _rolRepository = rolRepository;
    }

    public async Task<RolResponse> ExecuteAsync(CreateRolRequest request)
    {
        var rol = RolModel.Create(request.Name, request.Description);

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
