using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.ResponseDTO.Activation;
using RefundManagementApplication.Models.Enums;
using System.Diagnostics.CodeAnalysis;

namespace RefundManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivateController : ControllerBase
    {
        private readonly IActivateServices _service;
        private readonly ILogger<ActivateController> _logger;

        public ActivateController(IActivateServices service,ILogger<ActivateController> logger) {
            _service = service;
            _logger = logger;
        }

        [HttpPut]
        [Route("Activate")]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(typeof(ActivateReturnDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel),StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<ActivateReturnDTO>> ActivateUser(int MemberId,MemberRole role,Plan plan)
        {
            try
            {
                var res = await _service.Activate(MemberId,role,plan);
                _logger.LogInformation($"Activating the Member : {MemberId}");
                return Ok(res);
            }
            catch (NotFoundException unfe) {
                _logger.LogError(unfe.Message);
                return BadRequest(new ErrorModel(404,unfe.Message));
            }
        }



        [HttpPut]
        [Route("Deactivate")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ActivateReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]

        public async Task<ActionResult<ActivateReturnDTO>> DeactivateUser(int MemberId)
        {
            try
            {
                var res = await _service.Deactivate(MemberId);
                _logger.LogInformation($"Deactivating the Member : {MemberId}");
                return Ok(res);
            }
            catch (NotFoundException unfe)
            {
                _logger.LogError(unfe.Message);
                return BadRequest(new ErrorModel(404, unfe.Message));
            }
        }
    }
}
