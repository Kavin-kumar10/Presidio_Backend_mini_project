using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.RequestDTO.AuthReqDTOs;
using RefundManagementApplication.Models.DTOs.ResponseDTO.LoginResponseDTOs;

namespace RefundManagementApplication.Controllers
{


    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserServices _service;

        public UserController(IUserServices service) { 
            _service = service;
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType (typeof(Member),StatusCodes.Status200OK)]
        [ProducesResponseType (typeof(ErrorModel),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Member>> Register(RegisterRequestDTO registerrequestDTO)
        {
            try
            {
                var result =  await _service.Register(registerrequestDTO);
                return Ok(result);
            }
            catch (UnableToRegisterException utre) {
                return BadRequest(new ErrorModel(401, utre.Message));
            }
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(Member), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginReturnDTO>> Login(UserLoginDTO userloginDTO)
        {
            try
            {
                var result = await _service.Login(userloginDTO);
                return Ok(result);
            }
            catch (UserNotActiveException unae)
            {
                return BadRequest(unae.Message);
            }
            catch(UnauthorizedUserException uaue)
            {
                return BadRequest(uaue.Message);
            }
        }
    }
}
