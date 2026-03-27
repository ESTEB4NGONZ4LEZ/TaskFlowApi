using System.Security.Cryptography;
using Application.Services;

namespace Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(HashSize);

        var result = new byte[SaltSize + HashSize];
        Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
        Buffer.BlockCopy(hash, 0, result, SaltSize, HashSize);

        return Convert.ToBase64String(result);
    }

    public bool Verify(string password, string storedHash)
    {
        var bytes = Convert.FromBase64String(storedHash);
        var salt = bytes[..SaltSize];
        var expectedHash = bytes[SaltSize..];

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        var actualHash = pbkdf2.GetBytes(HashSize);

        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }
}
