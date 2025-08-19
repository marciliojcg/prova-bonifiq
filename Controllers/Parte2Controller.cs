using Microsoft.AspNetCore.Mvc;
using ProvaPub.Services.Interfaces;

namespace ProvaPub.Controllers
{
	
	[ApiController]
	[Route("[controller]")]
	public class Parte2Controller :  ControllerBase
	{
        private readonly IProductService _productService;
		private readonly ICustomerService _customerService;
        public Parte2Controller(ICustomerService customerService, IProductService productService)
		{
			_customerService = customerService;
			_productService = productService;
		}

        [HttpGet("products")]
        public async Task<IActionResult> ListProducts(int page, int pageSize = 10)
        {
            try
            {
                var products = await _productService.ListAsync(page, pageSize);

                if (products.Items.Count != 0)
                {
                    return Ok(products);
                }

                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("customers")]
        public async Task<IActionResult> ListCustomers(int page, int pageSize = 10)
        {
            try
            {

                var customers = await _customerService.ListAsync(page, pageSize);

                if (customers.Items.Count != 0)
                {
                    return Ok(customers);
                }

                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
