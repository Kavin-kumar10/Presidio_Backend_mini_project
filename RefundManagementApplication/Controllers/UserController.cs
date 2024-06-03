using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RefundManagementApplication.Exceptions.ActivationExceptions;
using RefundManagementApplication.Exceptions.AuthExceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.RequestDTO.AuthReqDTOs;
using RefundManagementApplication.Models.DTOs.ResponseDTO.LoginResponseDTOs;
using System.Diagnostics.CodeAnalysis;

namespace RefundManagementApplication.Controllers
{


    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserServices _service;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserServices service,ILogger<UserController> logger) { 
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType (typeof(Member),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType (typeof(ErrorModel),StatusCodes.Status401Unauthorized)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<Member>> Register(RegisterRequestDTO registerrequestDTO)
        {
            try
            {
                var result =  await _service.Register(registerrequestDTO);
                _logger.LogInformation("Registering the User");
                return Ok(result);
            }
            catch(MemberWithMailIdAlreadyFound mmaf)
            {
                _logger.LogError(mmaf.Message);
                return BadRequest(new ErrorModel(403, mmaf.Message));
            }
            catch (UnableToRegisterException utre) {
                return BadRequest(new ErrorModel(401, utre.Message));
            }
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(Member), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<LoginReturnDTO>> Login(UserLoginDTO userloginDTO)
        {
            try
            {
                var result = await _service.Login(userloginDTO);
                _logger.LogInformation("Loggin in the User");
                return Ok(result);
            }
            catch (UserNotActiveException unae)
            {
                _logger.LogError(unae.Message);
                return BadRequest(unae.Message);
            }
            catch(UnauthorizedUserException uaue)
            {
                _logger.LogError(uaue.Message);
                return BadRequest(uaue.Message);
            }
        }
    }
}
