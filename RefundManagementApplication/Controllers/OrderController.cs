using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.RequestDTO.OrderReqDTOs;
using RefundManagementApplication.Models.Enums;

namespace RefundManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IServices<int, Order> _service;
        private readonly IServices<int,Product> _productService;
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderServices _orderService;

        public OrderController(IServices<int, Order> service,IServices<int,Product> productService,IOrderServices orderServices,ILogger<OrderController> logger)
        {
            _service = service;
            _orderService = orderServices;  
            _productService = productService;
            _logger = logger;
        }

        // Function Specific Operations

        [HttpGet]
        [Authorize(Roles = "Collector")]
        [Route("GetPendingRefund")]
        [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]

        // Get Pending Products only for Collector - for further product Retrival process
        public async Task<ActionResult<IEnumerable<Order>>> GetPendingRefund()
        {
            try
            {
                var result = await _orderService.GetAllRefundDecisionPendingOrders();
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

        // Get Pending Products only for Collector - for further product Retrival process
        public async Task<ActionResult<IEnumerable<Order>>> GetAcceptedRefund()
        {
            try
            {
                var result = await _orderService.GetAllRefundDecisionAcceptedOrders();
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
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Order>> Create(OrderRequestDTO orderRequestDTO)
        {
            try
            {
                var product = await _productService.GetById(orderRequestDTO.ProductId);
                product.Count--;
                Order order = new Order() {
                    MemberID = orderRequestDTO.MemberID,
                    ProductId = orderRequestDTO.ProductId,
                    TotalPrice = product.Curr_price,
                };
                var result = await _service.Create(order);
                await _productService.Update(product);
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