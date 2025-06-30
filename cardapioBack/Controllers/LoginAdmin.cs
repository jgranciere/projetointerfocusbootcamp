using AprendendoAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AprendendoAPI.Controllers
{

    [ApiController]
    [Route("admin/login")]
    public class LoginAdmin : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;

        public LoginAdmin(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpPost]
        public IActionResult ValidarLoginAdmin([FromBody] LoginAcesso request)
        {
            if (request.Email == "admin" && request.Password == "admin")
            {
                var response = new LoginResponse
                {
                    Token = "token-imaginario",
                    Mensagem = "Login válido"
                };

                return Ok(response);
            }
            return Unauthorized(new LoginResponse { Mensagem = "Login invalido" });
        }


    }
}
