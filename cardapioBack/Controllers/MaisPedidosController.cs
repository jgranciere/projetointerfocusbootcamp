using AprendendoAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AprendendoAPI.Controllers
{

    [ApiController]
    [Route("api/maisPedidos")]
    public class MaisPedidosController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;

        public MaisPedidosController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpGet]
        public IActionResult GetMaisPedidos()
        {
            var maisPedidos = _produtoRepository.GetTodos();

            var produtos = maisPedidos.Select(mp => _produtoRepository.BuscarPorId(mp.ProdutoId))
                .Where(p => p != null)
                .ToList();

            return Ok(produtos);
        }

        [HttpPost("{id}")]
        public IActionResult MarcarComoMaisPedido(int id)
        {
            var produto = _produtoRepository.BuscarPorId(id);

            if (produto == null)
            {
                return NotFound();
            }

            var maisPedido = new ProdutoMaisPedido { ProdutoId = id };

            maisPedido.Categoria = "produtoSelecionado";

            _produtoRepository.AddProdutoMaisPedido(maisPedido);
            return Ok(maisPedido);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarProdutoMaisPedido(int id)
        {
            var produtoMaisPedido = _produtoRepository.BuscarMaisPedidoPorProdutoId(id);

            if (produtoMaisPedido == null)
            {
                return BadRequest("Produto não encontrado");
            }

            _produtoRepository.DeletarProdutoMaisPedido(produtoMaisPedido);
            return Ok("removido com sucesso");
        }
    }
}
