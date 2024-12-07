using Microsoft.AspNetCore.Identity;

namespace AradaAPI.Repositories.Interface
{
    public interface ITokenRepository
    {

        string CreateJWTToken(IdentityUser user, string role);
    }
}
