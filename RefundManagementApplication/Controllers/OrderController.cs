using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.RequestDTO.OrderReqDTOs;
using RefundManagementApplication.Models.Enums;
using System.Diagnostics.CodeAnalysis;

namespace RefundManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IServices<int, Order> _service;
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderServices _orderService;

        public OrderController(IServices<int, Order> service,IOrderServices orderServices,ILogger<OrderController> logger)
        {
            _service = service;
            _orderService = orderServices;  
            _logger = logger;
        }

        // Function Specific Operations

        [HttpGet]
        [Authorize(Roles = "Collector")]
        [Route("GetPendingRefund")]
        [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]

        // Get Pending Products only for Collector - for further product Retrival process
        public async Task<ActionResult<IEnumerable<Order>>> GetPendingRefund()
        {
            try
            {
                var result = await _orderService.GetAllRefundDecisionPendingOrders();
                _logger.LogInformation("Getting Decision Pending Requests");
                return Ok(result);
            }
            catch (NotFoundException nfe)
            {
                _logger.LogError(nfe.Message);
                return BadRequest(new ErrorModel(404, nfe.Message));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetAcceptedRefund")]
        [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]

        // Get Pending Products only for Collector - for further product Retrival process
        public async Task<ActionResult<IEnumerable<Order>>> GetAcceptedRefund()
        {
            try
            {
                var result = await _orderService.GetAllRefundDecisionAcceptedOrders();
                _logger.LogInformation("Getting Decision Accepted Requests");
                return Ok(result);
            }
            catch (NotFoundException nfe)
            {
                _logger.LogError(nfe.Message);
                return BadRequest(new ErrorModel(404, nfe.Message));
            }
        }


        [HttpPost]
        [Route("RefundDecision")]
        [Authorize(Roles = "Collector")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]

        // Accept or Reject the Product
        public async Task<ActionResult<Order>> RefundRequestDecision(int OrderId,bool decision)
        {
            try
            {
                var reqOrder = await _service.GetById(OrderId);
                if (decision)
                    reqOrder.OrderStatus = OrderStatuses.Refund_Accepted;
                else
                    reqOrder.OrderStatus = OrderStatuses.Refund_Rejected;
                var result = await _service.Update(reqOrder);
                return Ok(result);
            }
            catch(NotFoundException nfe)
            {
                _logger.LogError(nfe.Message);
                return BadRequest(new ErrorModel(404,nfe.Message));
            }
        }



        // Basic Crud operation

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]

        public async Task<ActionResult<IEnumerable<Order>>> Get()
        {
            try
            {
                var result = await _service.GetAll();
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
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]

        public async Task<ActionResult<Order>> GetById(int Id)
        {
            try
            {
                var result = await _service.GetById(Id);
                return Ok(result);
            }
            catch (UnableToCreateException utce)
            {
                _logger.LogError(utce.Message);
                return BadRequest(new ErrorModel(404, utce.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles ="User")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<Order>> Create(OrderRequestDTO orderRequestDTO)
        {
            try
            {
                var result = await _orderService.CreateOrder(orderRequestDTO);
                return Ok(result);
            }
            catch (UnableToCreateException utce)
            {
                _logger.LogError(utce.Message);
                return BadRequest(new ErrorModel(404, utce.Message));
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<Order>> Update(Order order)
        {
            try
            {
                var result = await _service.Update(order);
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
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<Order>> Delete(int Key)
        {
            try
            {
                var result = await _service.Delete(Key);
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