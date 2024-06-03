using Microsoft.AspNetCore.Authorization;
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
    public class ProductController : ControllerBase
    {
        IServices<int, Product> _service;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IServices<int,Product> service,ILogger<ProductController> logger) { 
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel),StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            try
            {
                var result = await _service.GetAll();
                _logger.LogInformation("Geting All Products");
                return Ok(result);
            }
            catch (NotFoundException nfe)
            {
                _logger.LogError(nfe.Message);
                return BadRequest(new ErrorModel(404,nfe.Message));
            }
        }

        [HttpGet]
        [Route("GetProductsById")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<Product>> GetById(int ProductId)
        {
            try
            {
                var result = await _service.GetById(ProductId);
                _logger.LogInformation($"Getting product by id : {ProductId}");
                return Ok(result);
            }
            catch (NotFoundException nfe)
            {
                _logger.LogError(nfe.Message);
                return BadRequest(new ErrorModel(404, nfe.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("MultiData")]
        [ProducesResponseType(typeof(IList<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<IList<Product>>> CreateMultipleEntity(IList<Product> products)
        {
            try
            {
                var result = await _service.CreateMultiple(products);
                _logger.LogInformation("Creating multiple products");
                return Ok(result);
            }
            catch (UnableToCreateException utce)
            {
                _logger.LogError(utce.Message);
                return BadRequest(new ErrorModel(404, utce.Message));
            }
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            try
            {
                var result = await _service.Create(product);
                _logger.LogInformation("Creating single product");
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
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<Product>> Update(Product product)
        {
            try
            {
                var result = await _service.Update(product);
                _logger.LogInformation("Updating product");
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
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ExcludeFromCodeCoverage]
        public async Task<ActionResult<Product>> Delete(int productKey)
        {
            try
            {
                var result = await _service.Delete(productKey);
                _logger.LogInformation("Deleting product");
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
