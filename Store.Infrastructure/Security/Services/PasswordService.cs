using Microsoft.AspNetCore.Identity;
using Store.Domain.Secutiry;

namespace Store.Infrastructure.Security.Services;

public class PasswordService : IPasswordService
{
    private static PasswordHasher<string> _passwordHasher = new();

    public static string HashPassword(string password)
        => _passwordHasher.HashPassword(null, password);

    public static bool IsValidPassword(string password, string hashedPassword)
        => _passwordHasher.VerifyHashedPassword(null, hashedPassword, password) == PasswordVerificationResult.Success;
}