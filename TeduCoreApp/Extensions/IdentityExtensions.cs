using System.Linq;
using System.Security.Claims;

namespace TeduCoreApp.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetSpecificClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var claims = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType);
            return claims.Value ?? string.Empty;
        }
    }
}
