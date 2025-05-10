using System.Net;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZdeskUserPortal.Business.Interface;
using ZdeskUserPortal.Business.Services;
using ZdeskUserPortal.DTOModel;
using ZdeskUserPortalApiCore.Common;
using ZdeskUserPortalApiCore.DTOModel;
using ZdeskUserPortalApiCore.JWTToken;
using Microsoft.AspNetCore.Authorization;
using ZdeskUserPortal.Domain.Model.Master;

namespace ZdeskUserPortalApiCore.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class DashBoardController : ControllerBase
    {
        private readonly ILogger<DashBoardController> _logger;
        private readonly IMaster _master;
        public DashBoardController(ILogger<DashBoardController> logger, IMaster master)
        {
            _logger = logger;
            _master = master;
        }
        [Microsoft.AspNetCore.Mvc.HttpGet(Name = "BusinessUnit")]
        [ProducesResponseType<ResponseMetaData<IEnumerable<BusinessUnitEntity>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ResponseMetaData<IEnumerable<BusinessUnitEntity>>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ResponseMetaData<IEnumerable<BusinessUnitEntity>>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BusinessUnit()
        {
            var responseMetadata = new ResponseMetaData<IEnumerable<BusinessUnitEntity>>();
            IEnumerable<BusinessUnitEntity> businessUnits;
            businessUnits = await _master.GetAllBusinessUnit();

            responseMetadata = new ResponseMetaData<IEnumerable<BusinessUnitEntity>>()
            {
                Status = HttpStatusCode.OK,
                IsError = false,
                Result = businessUnits,
                Message = "Business Unit Data Fetch Successfully!"
            };

            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }

    }
}
