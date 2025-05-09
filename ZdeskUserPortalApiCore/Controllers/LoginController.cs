using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZdeskUserPortal.Business.Interface;
using ZdeskUserPortal.DTOModel;
using ZdeskUserPortalApiCore.Common;
using ZdeskUserPortalApiCore.DTOModel;
using ZdeskUserPortalApiCore.JWTToken;

namespace ZdeskUserPortalApiCore.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly AuthService _authService;
        private readonly ILogin _loginService;
        public LoginController(ILogger<LoginController> logger1,AuthService authService,ILogin login)
        {
            _logger = logger1;
            _authService = authService;
            _loginService= login;
        }


        [HttpPost(Name = "Login")]
        [ProducesResponseType<ResponseMetaData<TokenDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ResponseMetaData<TokenDTO>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ResponseMetaData<TokenDTO>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var responseMetadata = new ResponseMetaData<TokenDTO>();
            TokenDTO tokenDTO = new TokenDTO();
            var result= await _loginService.UserLogin(loginDTO.Email, loginDTO.Password);
            if (result == 0)
            {
                responseMetadata = new ResponseMetaData<TokenDTO>()
                {
                    ErrorDetails = "User Id and Password is wrong!",
                    IsError = true,
                    Status = HttpStatusCode.Unauthorized,
                    Result= tokenDTO

                };
            }
            else
            {
                var user = new UserToken(result, loginDTO.Email, Roles: new[] { "Admin", "User" });
                var token = _authService.GenerateToken(user);
                tokenDTO= new TokenDTO() { Token=token};
                responseMetadata = new ResponseMetaData<TokenDTO>()
                {
                    Status = HttpStatusCode.OK,
                    IsError = false,
                    Result = tokenDTO,
                    Message="User Logged In Successfully!"
                };
            }
            
            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }
    }
}
