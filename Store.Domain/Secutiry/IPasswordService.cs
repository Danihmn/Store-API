namespace Store.Domain.Secutiry;

public interface IPasswordService
{
    static abstract string HashPassword(string password);
    static abstract bool IsValidPassword(string password, string hashedPassword);
}