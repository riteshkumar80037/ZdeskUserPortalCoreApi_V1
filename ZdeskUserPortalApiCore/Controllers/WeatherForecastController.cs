using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZdeskUserPortal.DataAccess.Home;
using ZdeskUserPortalApiCore.Common;
using ZdeskUserPortalApiCore.DTOModel;
using ZdeskUserPortalApiCore.JWTToken;

namespace ZdeskUserPortalApiCore.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly AuthService _authService;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, AuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [ProducesResponseType<ResponseMetaData<List<WeatherForecast>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ResponseMetaData<List<WeatherForecast>>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ResponseMetaData<List<WeatherForecast>>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            //var v = Convert.ToInt16("test");
            
           
            var user = new UserToken( "Ritesh@gmail.com","123456", Roles: new[] { "Admin", "User" });
          var token= _authService.GenerateToken(user);
            var responseMetadata = new ResponseMetaData<string>()
            {
                Status = HttpStatusCode.OK,
                IsError = false,
                Result= token
            };
            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }
        [HttpPost(Name = "Test")]
        [Authorize]
        [ProducesResponseType<ResponseMetaData<List<WeatherForecast>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ResponseMetaData<List<WeatherForecast>>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ResponseMetaData<List<WeatherForecast>>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Test(string t)
        {
              var responseMetadata = new ResponseMetaData<string>()
            {
                Status = HttpStatusCode.OK,
                IsError = false
            };
            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }
    }
}
