using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Exceptions.RefundExceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.RequestDTO.OrderReqDTOs;
using RefundManagementApplication.Models.Enums;
using RefundManagementApplication.Services;

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

        public RefundController(IOrderServices orderServices,IServices<int,Refund> service, IServices<int, Order> orderBaseService,IRefundServices refundServices)
        {
            _orderServices = orderServices;
            _service = service;
            _orderBaseServices = orderBaseService;
            _refundServices = refundServices;
        }


        //Function Specific Controllers



        // Base CRUD Controllers

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Refund>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Refund>>> Get()
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
        [ProducesResponseType(typeof(Refund), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Refund>> GetById(int Id)
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
        [ProducesResponseType(typeof(Refund), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Refund>> Create(int OrderId,String Reason)
        {
            try
            {
                //Order order = await _orderBaseServices.GetById(OrderId);
                //Refund refund = new Refund() { 
                //    OrderId = order.OrderId,
                //    CreatedBy = order.MemberID,
                //    CreatedDate = DateTime.Now,
                //    Reason = Reason,
                //    RefundAmount = order.TotalPrice,
                //    RefundStatus = RefundStatuses.PENDING
                //};
                //var result = await _service.Create(refund);
                var result = await _refundServices.CreateRefund(OrderId, Reason);
                await _orderServices.UpdateOrderStatus(OrderStatuses.Refund_Initiated,OrderId);
                return Ok(result);
            }
            catch (UnableToCreateException utce)
            {
                return BadRequest(new ErrorModel(404, utce.Message));
            }
            catch(ObjectIsNotReturnableException onre)
            {
                return BadRequest(new ErrorModel(405,onre.Message));
            }
            catch(ReturnableDateExpired rde)
            {
                return BadRequest(new ErrorModel(405,rde.Message));
            }
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Refund), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Refund>> Update(Refund refund)
        {
            try
            {
                var result = await _service.Update(refund);
                return Ok(result);
            }
            catch (NotFoundException nfe)
            {
                return BadRequest(new ErrorModel(404, nfe.Message));
            }
        }


        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Refund), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Refund>> Delete(int Key)
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
