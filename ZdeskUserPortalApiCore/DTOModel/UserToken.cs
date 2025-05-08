namespace ZdeskUserPortalApiCore.DTOModel
{
  
    public record UserToken(string Name, string Email, string Password, string[] Roles);
}
