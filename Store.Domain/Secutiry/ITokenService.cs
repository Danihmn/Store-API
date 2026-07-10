using Store.Domain.Entities;

namespace Store.Domain.Secutiry;

public interface ITokenService
{
    string GenerateToken (User user);
}