using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Exceptions.RefundExceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.RequestDTO.OrderReqDTOs;
using RefundManagementApplication.Models.Enums;
using RefundManagementApplication.Services;
using System.Diagnostics.CodeAnalysis;

namespace RefundManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundController : ControllerBase
    {
        private IOrderServices _orderServices;
        private IServices<int, Order> _orderBaseServices;
        private IServices<int, Refund> _service;
        private IRefundServices _refundServices;
        private ILogger<RefundController> _logger;

        public RefundController(IOrderServices orderServices,IServices<int,Refund> service, IServices<int, Order> orderBaseService,IRefundServices refundServices,ILogger<RefundController> logger)
        {
            _orderServices = orderServices;
            _service = service;
            _orderBaseServices = orderBaseService;
            _refundServices = refundServices;
            _logger = logger;
        }


        // Base CRUD Controllers

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Refund>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<IEnumerable<Refund>>> Get()
        {
            try
            {
                var result = await _service.GetAll();
                _logger.LogInformation("Getting all the refunds");
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
        [ProducesResponseType(typeof(Refund), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<Refund>> GetById(int Id)
        {
            try
            {
                var result = await _service.GetById(Id);
                _logger.LogInformation("Getting refund by Id");
                return Ok(result);
            }
            catch (UnableToCreateException utce)
            {
                _logger.LogError(utce.Message);
                return BadRequest(new ErrorModel(404, utce.Message));
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(Refund), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]   
        public async Task<ActionResult<Refund>> Create(int OrderId,String Reason)
        {
            try
            {
                var result = await _refundServices.CreateRefund(OrderId, Reason);
                await _orderServices.UpdateOrderStatus(OrderStatuses.Refund_Initiated,OrderId);
                _logger.LogInformation($"Creating Refund with order id {OrderId}");
                return Ok(result);
            }
            catch (UnableToCreateException utce)
            {
                _logger.LogError(utce.Message);
                return BadRequest(new ErrorModel(404, utce.Message));
            }
            catch(ObjectIsNotReturnableException onre)
            {   
                _logger.LogError(onre.Message);
                return BadRequest(new ErrorModel(405,onre.Message));
            }
            catch(ReturnableDateExpired rde)
            {
                _logger.LogError(rde.Message);
                return BadRequest(new ErrorModel(405,rde.Message));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(404,ex.Message));  
            }
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Refund), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<Refund>> Update(Refund refund)
        {
            try
            {
                var result = await _service.Update(refund);
                _logger.LogInformation("Updating Refund data");
                return Ok(result);
            }
            catch (NotFoundException nfe)
            {
                _logger.LogError(nfe.Message);
                return BadRequest(new ErrorModel(404, nfe.Message));
            }
        }


        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Refund), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<Refund>> Delete(int Key)
        {
            try
            {
                var result = await _service.Delete(Key);
                _logger.LogInformation("Deleting Refund");
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
