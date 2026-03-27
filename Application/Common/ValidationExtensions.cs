using FluentValidation;

namespace Application.Common;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> Required<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().WithMessage("{PropertyName} is required.");

    public static IRuleBuilderOptions<T, string> MaxLength<T>(this IRuleBuilder<T, string> rule, int max) =>
        rule.MaximumLength(max).WithMessage($"{{PropertyName}} must not exceed {max} characters.");

    public static IRuleBuilderOptions<T, string> MinLength<T>(this IRuleBuilder<T, string> rule, int min) =>
        rule.MinimumLength(min).WithMessage($"{{PropertyName}} must be at least {min} characters.");

    public static IRuleBuilderOptions<T, string> ValidEmail<T>(this IRuleBuilder<T, string> rule) =>
        rule.EmailAddress().WithMessage("{PropertyName} must be a valid email address.");

    public static IRuleBuilderOptions<T, string> ValidPassword<T>(this IRuleBuilder<T, string> rule) =>
        rule.Must(p => p != null
                    && p.Any(char.IsUpper)
                    && p.Any(char.IsLower)
                    && p.Any(char.IsDigit)
                    && p.Any(c => !char.IsLetterOrDigit(c)))
            .WithMessage("{PropertyName} must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
}
