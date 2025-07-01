using System.Globalization;
using AprendendoAPI.Models;
using AprendendoAPI.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AprendendoAPI.Controllers
{

    [ApiController]
    [Route("api/bebida")]
    public class BebidasController : ControllerBase
    {

        private readonly IProdutoRepository _produtoRepository;

        public BebidasController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpGet]
        public IActionResult GetBebidas()
        {
            var bebidas = _produtoRepository.Get().Where(p => p.Categoria == "bebida").ToList();
            return Ok(bebidas);
        }

        [HttpGet("{id}")]
        public IActionResult GetBebidasPorId(int id)
        {
            var bebida = _produtoRepository.BuscarPorId(id);

            if (bebida == null || bebida.Categoria != "bebida")
            {
                return BadRequest("Bebida nao encontrada");
            }

            return Ok(bebida);
        }

        [HttpPost]
        public IActionResult AddBebida([FromForm] ProdutoViewModel produtoView)
        {

            if (produtoView.Descricao != null && produtoView.Descricao.Length > 100)
            {
                return BadRequest(new { mensagemErro = "A descrição deve ter no máximo 100 caracteres" });
            }

            if (produtoView.foto == null || produtoView.foto.Length == 0)
            {
                return BadRequest(new { mensagemErro = "A foto do produto é obrigatória." });
            }

            var filePath = Path.Combine("Storage", produtoView.foto.FileName);

            using Stream fileStream = new FileStream(filePath, FileMode.Create);
            produtoView.foto.CopyTo(fileStream);

            var imagemUrl = $"https://localhost:7027/imagens/{produtoView.foto.FileName}";

            var bebida = new Produto(
                produtoView.Nome, 
                produtoView.Preco, 
                produtoView.Descricao, 
                imagemUrl,
                produtoView.QuantidadeMaxima,
                produtoView.Status
            );

            bebida.Categoria = "bebida";

            _produtoRepository.Add(bebida);

            var response = new
            {
                id = bebida.Id,
                nome = bebida.Nome,
                descricao = bebida.Descricao,
                preco = bebida.Preco.ToString("N2", new CultureInfo("pt-BR")),
                imagemUrl = bebida.ImagemUrl,
                quantidadeMaxima = bebida.QuantidadeMaxima,
                status = bebida.Status
            };

            return Ok(new { mensagemSucesso = $"Produto {bebida.Nome} cadastrado com sucesso!" });
        }

    }
}
