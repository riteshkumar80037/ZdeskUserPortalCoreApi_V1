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
        [ProducesResponseType<ResponseMetaData<WeatherForecast>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ResponseMetaData<WeatherForecast>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ResponseMetaData<WeatherForecast>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {

            var result= _loginService.UserLogin(loginDTO.EmailId, loginDTO.Password);
            var user = new UserToken(loginDTO.EmailId, loginDTO.Password, Roles: new[] { "Admin", "User" });
            var token = _authService.GenerateToken(user);
            var responseMetadata = new ResponseMetaData<string>()
            {
                Status = HttpStatusCode.OK,
                IsError = false,
                Result = token
            };
            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }
    }
}
