using System.Numerics;

namespace ZdeskUserPortalApiCore.DTOModel
{
  
    public record UserToken(BigInteger Id,string Email,string[] Roles);
}
