using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Exceptions.ProductExceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.ResponseDTO.Activation;

namespace RefundManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivateController : ControllerBase
    {
        private readonly IActivateServices _service;
        public ActivateController(IActivateServices service) {
            _service = service;
        }

        [HttpPut]
        [Route("Activate")]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(typeof(ActivateReturnDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel),StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ActivateReturnDTO>> ActivateUser(int MemberId)
        {
            try
            {
                var res = await _service.Activate(MemberId);
                return Ok(res);
            }
            catch (ProductNotFoundException unfe) {
                return BadRequest(new ErrorModel(404,unfe.Message));
            }
        }



        [HttpPut]
        [Route("Deactivate")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ActivateReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ActivateReturnDTO>> DeactivateUser(int MemberId)
        {
            try
            {
                var res = await _service.Deactivate(MemberId);
                return Ok(res);
            }
            catch (ProductNotFoundException unfe)
            {
                return BadRequest(new ErrorModel(404, unfe.Message));
            }
        }
    }
}
