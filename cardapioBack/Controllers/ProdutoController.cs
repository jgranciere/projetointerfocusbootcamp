using AprendendoAPI.Models;
using AprendendoAPI.ViewModel;
using AprendendoAPI.Infraestrutura;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AprendendoAPI.Controllers
{
    [ApiController]
    [Route("api/produto")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;
        
        public ProdutoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;   
        }

        [HttpGet]
        public IActionResult Get(
            [FromQuery] string sortBy = "nome",
            [FromQuery] string sortOrder = "asc",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 12,
            [FromQuery] string searchTerm = ""
        )
        {
            IQueryable<Produto> todosOsProdutos = _produtoRepository.Get();

            IQueryable<Produto> produtosFiltrados = todosOsProdutos;

            if (!string.IsNullOrEmpty(searchTerm))
            {

                string termoDeBuscaEmMinusculas = searchTerm.ToLower();

                produtosFiltrados = produtosFiltrados.Where(p=> p.Nome.ToLower().Contains(termoDeBuscaEmMinusculas) ||
                                p.Descricao.ToLower().Contains(termoDeBuscaEmMinusculas));
            }

            IQueryable<Produto> produtosOrdenados = produtosFiltrados;

            switch (sortBy.ToLower())
            {
                case "nome":
                    if(sortOrder.ToLower() == "desc")
                    {
                        produtosOrdenados = produtosOrdenados.OrderByDescending(p => p.Nome);
                    }
                    else
                    {
                        produtosOrdenados = produtosOrdenados.OrderBy(p => p.Nome);
                    }
                    break;
                case "preco":
                    if(sortOrder.ToLower() == "desc")
                    {
                        produtosOrdenados = produtosOrdenados.OrderByDescending(p => p.Preco);
                    }
                    else
                    {
                        produtosOrdenados = produtosOrdenados.OrderBy(p => p.Preco);
                    }
                    break;
                default:
                    produtosOrdenados = produtosOrdenados.OrderBy(p => p.Nome);
                    break;
            }

            int totalDeItens = produtosOrdenados.Count();

            int itensParaPular = (page - 1) * pageSize;

            List<Produto> produtosDestaPagina = produtosOrdenados
                .Skip(itensParaPular)
                .Take(pageSize)
                .ToList();


            return Ok(new
            {
                Products = produtosDestaPagina,
                TotalCount = totalDeItens,
                CurrentPage = page,
                PageSize = pageSize
            });
        }


        [HttpPost]
        public IActionResult Add([FromForm] ProdutoViewModel produtoView)
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

            var produto = new Produto(
                produtoView.Nome,
                produtoView.Preco, 
                produtoView.Descricao, 
                imagemUrl,
                produtoView.QuantidadeMaxima,
                produtoView.Status
            );

            produto.Categoria = "comida";


            _produtoRepository.Add(produto);

            var response = new
            {
                id = produto.Id,
                nome = produto.Nome,
                descricao = produto.Descricao,
                preco = produto.Preco.ToString("N2", new CultureInfo("pt-BR")),
                imagemUrl = produto.ImagemUrl,
                quantidadeMaxima = produto.QuantidadeMaxima,
                status = produto.Status
            };

            return Ok(new { mensagemSucesso = $"Produto {produto.Nome} cadastrado com sucesso!"});
        }

        [HttpGet("{id}")]
        public IActionResult GetPorId(int id)
        {
            var produto = _produtoRepository.BuscarPorId(id);

            if(produto == null)
            {
                return NotFound("Produto não encontrado.");
            }
            return Ok(produto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ProdutoViewModel produtoUpdateView)
        {
            var produto = _produtoRepository.BuscarPorId(id);

            if (produto == null)
            {
                return NotFound("Produto não encontrado.");
            }

            produto.Nome = produtoUpdateView.Nome;
            produto.Preco = produtoUpdateView.Preco;
            produto.Descricao = produtoUpdateView.Descricao;
            produto.QuantidadeMaxima = produtoUpdateView.QuantidadeMaxima;
            produto.Status = produtoUpdateView.Status;

            if (produto.Descricao != null && produto.Descricao.Length > 100)
            {
                return BadRequest(new { mensagemErro = "A descrição deve ter no máximo 100 caracteres" });
            }

            _produtoRepository.Update(produto);
            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var produto = _produtoRepository.BuscarPorId(id);

            if (produto == null)
            {
                return NotFound("Produto não encontrado.");
            }
            _produtoRepository.Delete(produto);
            return Ok();
        }
    }
}
