using Domain.Exceptions;

namespace Domain.Entities;

public class User
{
    public int UserId { get; private set; }
    public string UserName { get; private set; }
    public string Password { get; private set; }
    public string Email { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private User() { }

    public static User Create(string userName, string password, string email)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(userName))
            errors.Add("UserName is required.");
        else if (userName.Length > 50)
            errors.Add("UserName must not exceed 50 characters.");

        if (string.IsNullOrWhiteSpace(password))
            errors.Add("Password is required.");
        else if (password.Length > 255)
            errors.Add("Password must not exceed 255 characters.");

        if (string.IsNullOrWhiteSpace(email))
            errors.Add("Email is required.");
        else if (email.Length > 100)
            errors.Add("Email must not exceed 100 characters.");

        if (errors.Any())
            throw new ValidationException(errors);

        return new User
        {
            UserName = userName,
            Password = password,
            Email = email,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string userName, string email, bool isActive, string? password = null)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(userName))
            errors.Add("UserName is required.");
        else if (userName.Length > 50)
            errors.Add("UserName must not exceed 50 characters.");

        if (string.IsNullOrWhiteSpace(email))
            errors.Add("Email is required.");
        else if (email.Length > 100)
            errors.Add("Email must not exceed 100 characters.");

        if (password is not null && password.Length > 255)
            errors.Add("Password must not exceed 255 characters.");

        if (errors.Any())
            throw new ValidationException(errors);

        UserName = userName;
        Email = email;
        IsActive = isActive;
        if (password is not null)
            Password = password;
        UpdatedAt = DateTime.UtcNow;
    }

    public static User Reconstitute(int userId, string userName, string password, string email, bool isActive, DateTime createdAt, DateTime? updatedAt)
    {
        return new User
        {
            UserId = userId,
            UserName = userName,
            Password = password,
            Email = email,
            IsActive = isActive,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }
}
