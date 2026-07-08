using Microsoft.AspNetCore.Identity;

namespace Store.Infrastructure.Security.Services;

public class PasswordService
{
    private static PasswordHasher<string> _passwordHasher = new();

    public static string HashPassword (string password)
        => _passwordHasher.HashPassword(null, password);

    public static bool IsValidPassword (string password, string hashedPassword)
        => _passwordHasher.VerifyHashedPassword(null, hashedPassword, password) == PasswordVerificationResult.Success;
}