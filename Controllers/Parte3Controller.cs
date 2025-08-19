using Microsoft.AspNetCore.Mvc;
using ProvaPub.Contracts;
using ProvaPub.Services.Interfaces;

namespace ProvaPub.Controllers
{

    /// <summary>
    /// Esse teste simula um pagamento de uma compra.
    /// O método PayOrder aceita diversas formas de pagamento. Dentro desse método é feita uma estrutura de diversos "if" para cada um deles.
    /// Sabemos, no entanto, que esse formato não é adequado, em especial para futuras inclusões de formas de pagamento.
    /// Como você reestruturaria o método PayOrder para que ele ficasse mais aderente com as boas práticas de arquitetura de sistemas?
    /// 
    /// Outra parte importante é em relação à data (OrderDate) do objeto Order. Ela deve ser salva no banco como UTC mas deve retornar para o cliente no fuso horário do Brasil. 
    /// Demonstre como você faria isso.
    /// </summary>
    [ApiController]
	[Route("[controller]")]
	public class Parte3Controller :  ControllerBase
	{
        private readonly IOrderService _orderService;

        public Parte3Controller(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("orders")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequest request)
        {
            try
            {
                var order = await _orderService.PayOrder(
                    request.PaymentMethod,
                    request.PaymentValue,
                    request.CustomerId);

                // Converte UTC para horário brasileiro (UTC-3)
                var response = new OrderResponse
                {
                    Id = order.Id,
                    Value = order.Value,
                    CustomerId = order.CustomerId,
                    OrderDate = order.OrderDate.AddHours(-3), // UTC para UTC-3
                    PaymentMethod = request.PaymentMethod
                };

                return Ok(response);
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
