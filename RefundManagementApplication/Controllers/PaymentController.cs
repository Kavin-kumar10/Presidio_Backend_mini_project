using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using System.Diagnostics.CodeAnalysis;

namespace RefundManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        IPaymentServices _paymentServices;
        private ILogger<PaymentController> _logger;

        public PaymentController(IPaymentServices paymentServices,ILogger<PaymentController> logger) {
            _paymentServices = paymentServices;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Payment), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<Payment>> GetMyPaymnt(int PaymentId)
        {
            try
            {
                var result = await _paymentServices.GetMyPayment(PaymentId);
                _logger.LogInformation("Get Payment");
                return Ok(result);
            }
            catch(NotFoundException nfe) {

                _logger.LogError(nfe.Message);
                return BadRequest(new ErrorModel(404, nfe.Message));
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(Payment),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorModel),StatusCodes.Status401Unauthorized)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<Payment>> RefundPayment(int AdminId,int RefundId)
        {
            try
            {
                var result = await _paymentServices.CreatePaymentToRefund(AdminId,RefundId);
                _logger.LogInformation("Refund Payement Processing");
                return Ok(result);
            }
            catch(NotFoundException nfe)
            {
                _logger.LogError(nfe.Message);
                return BadRequest(new ErrorModel(404,nfe.Message));
            }
            catch (ForbiddenEntryException fee)
            {
                _logger.LogError(fee.Message);
                return BadRequest(new ErrorModel(403, fee.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(401,ex.Message));
            }
        }
    }
}
