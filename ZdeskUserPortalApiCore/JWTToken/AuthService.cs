using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZdeskUserPortalApiCore.DTOModel;

namespace ZdeskUserPortalApiCore.JWTToken
{
    public class AuthService
    {
        private readonly string _privateKey;

        public AuthService(IOptions<AuthSettings> options)
        {
            _privateKey = options.Value.PrivateKey;
        }
        public string GenerateToken(UserToken user)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_privateKey);
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = credentials,
                Issuer="abc",
                Audience="abc"
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(UserToken user)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            foreach (var role in user.Roles)
                claims.AddClaim(new Claim(ClaimTypes.Role, role));

            return claims;
        }
    }
}
