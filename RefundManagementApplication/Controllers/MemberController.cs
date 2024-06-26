using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Services;
using System.Diagnostics.CodeAnalysis;

namespace RefundManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IServices<int, Member> _service;
        private readonly ILogger<MemberController> _logger;


        public MemberController(IServices<int,Member> service, ILogger<MemberController> logger) { 
            _service = service;
            _logger = logger;   
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Member>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]

        public async Task<ActionResult<IEnumerable<Order>>> GetAllMembers()
        {
            try
            {
                var result = await _service.GetAll();
                _logger.LogInformation("Get the Member Details");
                return Ok(result);
            }
            catch (NotFoundException nfe)
            {
                _logger.LogError(nfe.Message);
                return BadRequest(new ErrorModel(404, nfe.Message));
            }
        }

        [HttpGet]
        [Route("GetById")]
        [ProducesResponseType(typeof(IEnumerable<Member>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]

        public async Task<ActionResult<IEnumerable<Order>>> GetMemberById(int memberId)
        {
            try
            {
                var result = await _service.GetById(memberId);
                _logger.LogInformation("Get the Member Detail by Id");
                return Ok(result);
            }
            catch (NotFoundException nfe)
            {
                _logger.LogError(nfe.Message);
                return BadRequest(new ErrorModel(404, nfe.Message));
            }
        }
    }
}
