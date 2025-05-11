using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ZdeskUserPortal.Business.Interface;
using ZdeskUserPortal.Domain.Model.Login;
using ZdeskUserPortal.Domain.Model.Master;
using ZdeskUserPortal.DTOModel;
using ZdeskUserPortalApiCore.Common;

namespace ZdeskUserPortalApiCore.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class MasterController : ControllerBase
    {
        private readonly ILogger<MasterController> _logger;
        private readonly IMaster _master;
        private readonly IMapper _mapper;
        public MasterController(ILogger<MasterController> logger, IMaster master,IMapper mapper)
        {
            _logger = logger;
            _master = master;
            _mapper = mapper;
        }


        [HttpGet("logo", Name = "Logo")]
        [ProducesResponseType<ResponseMetaData<LogoDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ResponseMetaData<LogoDTO>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ResponseMetaData<LogoDTO>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logo()
        {
            var responseMetadata = new ResponseMetaData<LogoDTO>();
            var result = await _master.OrganizationDetail();
            var logoResult = _mapper.Map<LogoDTO>(result);
            responseMetadata = new ResponseMetaData<LogoDTO>()
            {
                Status = HttpStatusCode.OK,
                IsError = false,
                Result = logoResult,
                Message = "Logo Data Fetch Successfully!"
            };

            return StatusCode((int)responseMetadata.Status, responseMetadata);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("businessUnit", Name = "BusinessUnit")]
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
