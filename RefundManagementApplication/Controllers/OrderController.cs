using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefundManagementApplication.Exceptions.ProductExceptions;
using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.RequestDTO.OrderReqDTOs;

namespace RefundManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IServices<int, Order> _service;
        private IServices<int,Product> _productService;

        public OrderController(IServices<int, Order> service,IServices<int,Product> productService)
        {
            _service = service;
            _productService = productService;
        }

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
                return BadRequest(new ErrorModel(404, nfe.Message));
            }
        }
    }
}