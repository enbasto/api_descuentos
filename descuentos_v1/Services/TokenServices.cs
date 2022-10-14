using System.Security.Claims;

namespace WSDISCOUNT.Services
{
    public class TokenServices
    {
        private readonly LoginServices _UsersService;
        public TokenServices(LoginServices UsersServices)
        {
            _UsersService = UsersServices;
        }
        public  dynamic validarToken(ClaimsIdentity identity)
        {
			try
			{
				if (identity.Claims.Count()==0)
				{
					return new { Validacion = false,
								Mesagge = "No Viene Token",
                                Result=""
								};
				}
                var Id = identity.Claims.FirstOrDefault(x=>x.Type=="Id").Value;
                var Registers =  _UsersService.GetUserValidation(Id);
                if (Registers is null)
                {
                    return new { Validacion = false, Mesagge = "Datos De Logeo Incorrectos", Result="" };
                }
                return new { Validacion = true, Mesagge = "Exito",Result=Registers };
            }
            catch (Exception e)
			{
                return new
                {
                    Validacion=false,
                    Mesagge="Se Presento una Excepcion: " + e.Message,
                    Result=""
                };
            }
        }
    }
}
