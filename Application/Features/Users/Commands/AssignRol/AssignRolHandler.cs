using Application.DTOs.User;
using Domain.Entities;
using Domain.Ports;
using Domain.Ports.Repositories;
using MediatR;

namespace Application.Features.Users.Commands.AssignRol;

public class AssignRolHandler : IRequestHandler<AssignRolCommand, UserRolResponse>
{
    private readonly IUserRolRepository _userRolRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignRolHandler(IUserRolRepository userRolRepository, IUnitOfWork unitOfWork)
    {
        _userRolRepository = userRolRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserRolResponse> Handle(AssignRolCommand command, CancellationToken cancellationToken)
    {
        var userRol = UserRol.Create(command.UserId, command.RolId);

        await _userRolRepository.AssignAsync(userRol);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new UserRolResponse(userRol.UserId, userRol.RolId, userRol.AssignedAt);
    }
}
