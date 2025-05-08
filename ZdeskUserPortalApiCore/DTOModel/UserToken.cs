namespace ZdeskUserPortalApiCore.DTOModel
{
  
    public record UserToken(string Email, string Password, string[] Roles);
}
