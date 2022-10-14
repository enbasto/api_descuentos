using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Security.Claims;
using WSDISCOUNT.Models;
using WSDISCOUNT.Services;

namespace WSDISCOUNT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : Controller
    {
        private readonly SalesServices _SalesService;
        private readonly LoginServices _UsersService;
        public SalesController(SalesServices SalesServices, LoginServices UsersServices){ 
            _SalesService=SalesServices;
            _UsersService=UsersServices;
        }
        [HttpGet]
        public async Task<ActionResult> QuerySales() 
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
                var Registers = await _SalesService.GetSales();
                if (Registers is not null)
                {
                    double TotalValue = Registers.Sum(item => item.Value);
                    double TotalDisconunts = Registers.Sum(item => item.Value_Paid_Out);
                    return Ok(new {Total_Descuentos_Dados = TotalValue - TotalDisconunts});
                }
                return Ok(new {Total_Descuentos_Dados = "No hay Datos Registrados"});
            }
            catch (Exception e)
            {   
                return StatusCode((int)HttpStatusCode.InternalServerError, "Ocurrio una Exepcion Inesperada : " + e.Message);
            }
            

        }
    }
}
