using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IServices<int, Product> _service;
        public ProductController(IServices<int,Product> service) { 
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            try
            {
                var result = await _service.GetAll();
                return Ok(result);
            }
            catch (NotFoundException nfe)
            {
                return BadRequest(new ErrorModel(404,nfe.Message));
            }
        }

        [HttpGet]
        [Route("GetProductsById")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetById(int ProductId)
        {
            try
            {
                var result = await _service.GetById(ProductId);
                return Ok(result);
            }
            catch (NotFoundException nfe)
            {
                return BadRequest(new ErrorModel(404, nfe.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("MultiData")]
        [ProducesResponseType(typeof(IList<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IList<Product>>> CreateMultipleEntity(IList<Product> products)
        {
            try
            {
                var result = await _service.CreateMultiple(products);
                return Ok(result);
            }
            catch (UnableToCreateException utce)
            {
                return BadRequest(new ErrorModel(404, utce.Message));
            }
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            try
            {
                var result = await _service.Create(product);
                return Ok(result);
            }
            catch (UnableToCreateException utce)
            {
                return BadRequest(new ErrorModel(404, utce.Message));
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> Update(Product product)
        {
            try
            {
                var result = await _service.Update(product);
                return Ok(result);
            }
            catch (NotFoundException nfe)
            {
                return BadRequest(new ErrorModel(404, nfe.Message));
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> Delete(int productKey)
        {
            try
            {
                var result = await _service.Delete(productKey);
                return Ok(result);
            }
            catch (NotFoundException nfe)
            {
                return BadRequest(new ErrorModel(404, nfe.Message));
            }
        }
    }
}
