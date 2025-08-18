using Microsoft.AspNetCore.Mvc;
using ProvaPub.Models;
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
        public async Task<PagedList<Product>> ListProducts(int page)
        {
			return await _productService.ListAsync(page, 10);
        }

        //      [HttpGet("customers")]
        //public CustomerList ListCustomers(int page)
        //{
        //	var customerService = new CustomerService(_ctx);
        //	return customerService.ListCustomers(page);
        //}

        [HttpGet("customers")]
        public async Task<PagedList<Customer>> ListCustomers(int page)
        {
            return await _customerService.ListAsync(page, 10);
        }
    }
}
