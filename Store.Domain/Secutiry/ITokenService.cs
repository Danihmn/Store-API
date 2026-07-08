using Store.Domain.Entities;

namespace Store.Domain.Secutiry;

public interface ITokenService
{
    string Create (User user);
}