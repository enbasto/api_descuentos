using WSDISCOUNT.Models;
using WSDISCOUNT.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace WSDISCOUNT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DescuentosController : Controller
    {
        private readonly DiscountServices _DescuentosService;
        private readonly SalesServices _SalesService;
        private readonly LoginServices _UsersService;
        public DescuentosController(DiscountServices DescuentosServices, SalesServices SalesServices, LoginServices UsersServices)
        {
            _DescuentosService = DescuentosServices;
            _SalesService = SalesServices;
            _UsersService = UsersServices;
        }
        [HttpPost]
        public async Task<ActionResult<ObjetDescuento>> QueryDiscount([FromBody] Discount discount)
        {
            try
            {
                var Identity = HttpContext.User.Identity as ClaimsIdentity;
                TokenServices tokenServices = new TokenServices(_UsersService);
                var Token = tokenServices.validarToken(Identity);
                if (!Token.Validacion)
                {
                   return Ok(Token);
                }
                var registroDescuento = await _DescuentosService.PostDiscount(discount.Consola);
                if (registroDescuento is null)
                {
                    return NotFound(new
                    {
                        title = $"No Se Encontro Descuentos Para {discount.Consola}",
                        status = 404,
                    });
                }
                if (discount.valor >= registroDescuento.Minimal_price && registroDescuento.Maximum_price <= discount.valor)
                {
                    var ResponseDiscount = ((discount.valor / 100) * -registroDescuento.Discount) + discount.valor;
                    Sales RegisterSales = new Sales
                                            {
                                                Console = discount.Consola,
                                                Value = discount.valor,
                                                Value_Paid_Out = ResponseDiscount
                                            };
                    var ResponseSales = _SalesService.CreateSalesAsync(RegisterSales);
                    if (ResponseSales != null)
                    {
                        return Ok(new { ValorCobrarCliente = ResponseDiscount });
                    }
                    return Ok(new { ValorCobrarCliente = "Error al Registrar la Venta" });
                }

                return Ok(new { ValorCobrarCliente = "No Tiene Descuento" });
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Ocurrio una Exepcion Inesperada : " + e.Message);
            }

        }
        
    }
}
