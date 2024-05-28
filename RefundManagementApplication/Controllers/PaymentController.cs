using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        IPaymentServices _paymentServices;
        public PaymentController(IPaymentServices paymentServices) {
            _paymentServices = paymentServices;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Payment),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorModel),StatusCodes.Status401Unauthorized)]    
        public async Task<ActionResult<Payment>> RefundPayment(int AdminId,int RefundId)
        {
            try
            {
                var result = await _paymentServices.CreatePaymentToRefund(AdminId,RefundId);
                return Ok(result);
            }
            catch(NotFoundException nfe)
            {
                return BadRequest(new ErrorModel(404,nfe.Message));
            }
            catch (ForbiddenEntryException fee)
            {
                return BadRequest(new ErrorModel(403, fee.Message));
            }
            catch (Exception ex)
            {
                 return BadRequest(new ErrorModel(401,ex.Message));
            }
        }
    }
}
