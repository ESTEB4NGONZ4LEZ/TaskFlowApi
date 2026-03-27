using Application.DTOs.User;
using Application.Services;
using Domain.Entities;
using Domain.Ports;
using Domain.Ports.Repositories;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var hashedPassword = _passwordHasher.Hash(command.Password);
        var user = User.Create(command.UserName, hashedPassword, command.Email);

        var getCreated = await _userRepository.CreateAsync(user);
        await _unitOfWork.CommitAsync(cancellationToken);
        var created = getCreated();

        return new UserResponse(created.UserId, created.UserName, created.Email, created.IsActive, created.CreatedAt, created.UpdatedAt);
    }
}
