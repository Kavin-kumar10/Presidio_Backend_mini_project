using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Exceptions.ProductExceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IProductServices _service;
        public ProductController(IProductServices service) { 
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            try
            {
                var result = await _service.GetAllProducts();
                return Ok(result);
            }
            catch (ProductNotFoundException pnfe)
            {
                return BadRequest(new ErrorModel(404,pnfe.Message));
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
                var result = await _service.GetProductById(ProductId);
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
                var result = await _service.CreateProduct(product);
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
                var result = await _service.UpdateProduct(product);
                return Ok(result);
            }
            catch (ProductNotFoundException pnfe)
            {
                return BadRequest(new ErrorModel(404, pnfe.Message));
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> Delete(Product product)
        {
            try
            {
                var result = await _service.DeleteProduct(product);
                return Ok(result);
            }
            catch (ProductNotFoundException pnfe)
            {
                return BadRequest(new ErrorModel(404, pnfe.Message));
            }
        }
    }
}
