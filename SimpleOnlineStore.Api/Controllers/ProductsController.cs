using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SimpleOnlineStore.Api.Domain.Dtos.Inventory.Requests;
using SimpleOnlineStore.Api.Domain.Dtos.Order.Requests;
using SimpleOnlineStore.Api.Domain.Dtos.Product.Requests;
using SimpleOnlineStore.Api.Helper.ExceptionHandlers;
using SimpleOnlineStore.Api.IServices;

namespace SimpleOnlineStore.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
    
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductRequestDto product,CancellationToken cancellationToken)
        {
            try
            {
                var createdProduct = await _productService.AddProductAsync(product,cancellationToken);
                return Ok(createdProduct);
            }

            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInventoryCount(UpdateInventoryCountRequestDto updateInventoryCountDto, CancellationToken cancellationToken)
        {
            try
            {
                var updatedProduct = await _productService.UpdateInventoryCountAsync(updateInventoryCountDto, cancellationToken);
                return Ok(updatedProduct);
            }
            catch (ProductNotFundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProductById([FromQuery] GetProductByIdRequestDto getProductByIdDto, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(getProductByIdDto, cancellationToken);
                return Ok(product);
            }
            catch (ProductNotFundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> BuyProduct(BuyProductRequestDto buyProductDto, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _productService.BuyProductAsync(buyProductDto,cancellationToken);
                return Ok(order);
            }
            catch (ProductNotFundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UserNotFundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
