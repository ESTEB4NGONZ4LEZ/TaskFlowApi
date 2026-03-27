using Application.DTOs.User;
using Application.Services;
using Domain.Exceptions;
using Domain.Ports;
using Domain.Ports.Repositories;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public UpdateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserResponse> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId)
            ?? throw new NotFoundException("User", command.UserId);

        var hashedPassword = command.Password is not null
            ? _passwordHasher.Hash(command.Password)
            : null;

        user.Update(command.UserName, command.Email, command.IsActive, hashedPassword);

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new UserResponse(user.UserId, user.UserName, user.Email, user.IsActive, user.CreatedAt, user.UpdatedAt);
    }
}
